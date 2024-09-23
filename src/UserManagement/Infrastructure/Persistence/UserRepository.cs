using Dapper;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence.Configurations;

namespace UserManagement.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private static async Task<IEnumerable<User>> GetUsers(string strParam = "", int? status = null)
        {
            using (var conn = DapperConnectionProvider.CreatePgConnection())
            {
                var users = (from user in conn.GetList<User>()
                                .Where(x => string.IsNullOrEmpty(strParam) || x.Id == strParam || x.Email == strParam || x.Username == strParam)
                                .Where(x => status is null || x.Status == status)
                             join role in from userRole in conn.GetList<UserRole>()
                                          join role in conn.GetList<Role>() on userRole.RoleId equals role.Id
                                          select new { userRole, role } on user.Id equals role.userRole.Id into role1
                             from role in role1.DefaultIfEmpty()
                             join userFile in conn.GetList<UserFile>() on user.Id equals userFile.Id into userFile1
                             from userFile in userFile1.DefaultIfEmpty()
                             select new { user, role?.role, userFile }
                            ).ToList().GroupBy(x => x.user).Select(g =>
                            {
                                var user = g.Select(x => x.user).First();
                                user.Roles = g.Where(x => x.role is not null).Select(x => x.role);
                                user.Files = g.Where(x => x.userFile is not null).Select(x => x.userFile);
                                return user;
                            });
                return await Task.FromResult(users);
            }
        }
        private static IEnumerable<ApplTask> FindAllParents(List<ApplTask> all_data, ApplTask child)
        {
            var parent = all_data.FirstOrDefault(x => x.Id == child.ApplTaskParentId);

            if (parent == null)
                return [];

            return new[] { parent }.Concat(FindAllParents(all_data, parent));
        }

        public void Add(User entity, Action<User> result)
        {
            using (var conn = DapperConnectionProvider.CreatePgConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    _ = conn.Insert<string, User>(entity);
                    if (entity.Files is not null)
                    {
                        foreach (var userFile in entity.Files)
                        {
                            _ = conn.Insert<string, UserFile>(userFile);
                        }
                    }
                    if (entity.Roles is not null)
                    {
                        List<UserRole> userRoles = entity.Roles.Select(x => new UserRole { Id = entity.Id, RoleId = x.Id }).ToList();
                        foreach (var userRole in userRoles)
                            _ = conn.Insert<string, UserRole>(userRole);
                    }
                    result(entity);
                    tx.Commit();
                }
            }
        }

        public async void Delete(string userId, string endpointName, string id, Action<string> result)
        {
            using (var conn = DapperConnectionProvider.CreatePgConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {

                    var userExist = await GetById(id);
                    userExist.SaveHistory(conn, userId, endpointName);

                    _ = conn.DeleteList<UserFile>(new { Id = id });
                    _ = conn.DeleteList<UserRole>(new { Id = id });
                    _ = conn.DeleteList<User>(new { Id = id });
                    result(string.Empty);
                    tx.Commit();
                }
            }
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return GetUsers();
        }

        public Task<User> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return Task.FromResult<User>(null);
            var data = GetUsers(strParam: id);
            var result = data.Result.Any() ? data.Result.First() : null;
            return Task.FromResult<User>(result);
        }
        public void Update(string userId, string endpointName, User entity, Action<User> result)
        {
            using (var conn = DapperConnectionProvider.CreatePgConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    var itemExist = conn.Get<User>(entity.Id);
                    itemExist.SaveHistory(conn, userId, endpointName);
                    _ = conn.Update(entity);
                    if (entity.Files is not null)
                    {
                        foreach (var userFile in entity.Files)
                        {
                            userFile.SaveHistory(conn, userId, endpointName);
                            conn.DeleteList<UserFile>(new { Id = userFile.Id, Type = userFile.Type });
                            _ = conn.Insert<string, UserFile>(userFile);
                        }
                    }
                    if (entity.Roles is not null)
                    {
                        var userRoleExist = conn.GetList<UserRole>()
                                            .Where(x => x.Id == entity.Id)
                                            .Select(x =>
                                            {
                                                x.SaveHistory(conn, userId, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}");
                                                return x;
                                            });
                        _ = conn.DeleteList<UserRole>(new { Id = entity.Id });
                        foreach (var item in entity.Roles)
                        {
                            UserRole userRole = new()
                            {
                                Id = entity.Id,
                                RoleId = item.Id
                            };
                            _ = conn.Insert<string, UserRole>(userRole);
                        }
                    }


                    result(entity);
                    tx.Commit();
                }
            }
        }
        public Task<User> GetUser(string idOrUsernameOrEmail)
        {
            if (string.IsNullOrEmpty(idOrUsernameOrEmail))
                return Task.FromResult<User>(null);
            var data = GetUsers(strParam: idOrUsernameOrEmail);
            var result = data.Result.Any() ? data.Result.First() : null;
            return Task.FromResult<User>(result);
        }
        public async Task<IEnumerable<ApplTask>> GetMenus(string languageId, string applId, string userId)
        {
            using var conn = DapperConnectionProvider.CreatePgConnection();
            var applTasks = (from applTask in conn.GetList<ApplTask>().Where(x => x.ApplId == applId)
                             join applTaskTr in conn.GetList<ApplTaskTr>().Where(x => x.LanguageId == languageId) on applTask.Id equals applTaskTr.Id into applTaskTr1
                             from applTaskTr in applTaskTr1.DefaultIfEmpty()
                             join appl in conn.GetList<Appl>() on applTask.ApplId equals appl.Id
                             join applTr in conn.GetList<ApplTr>().Where(x => x.LanguageId == languageId) on appl.Id equals applTr.Id into applTr1
                             from applTr in applTr1.DefaultIfEmpty()
                             join roleApplTask in conn.GetList<RoleApplTask>() on applTask.Id equals roleApplTask.ApplTaskId
                             join userRole in conn.GetList<UserRole>() on roleApplTask.Id equals userRole.RoleId
                             join user in conn.GetList<User>().Where(x => x.Id == userId) on userRole.Id equals user.Id
                             select new { applTask, applTaskTr, appl, applTr }
                            ).Select(x =>
                                {
                                    ApplTask applTask = x.applTask;
                                    applTask.Appl = x.appl;
                                    x.appl.SetTr(x.applTr);
                                    applTask.SetTr(x.applTaskTr);
                                    return applTask;
                                }).DistinctBy(x => x).OrderBy(x => x.Id);

            var applTaskDelegations = from a in conn.GetList<ApplTaskDelegation>()
                                        .Where(x => x.DelegateFor == userId)
                                        .Where(x => DateTime.Now.Date >= x.StartDate.Date & DateTime.Now.Date <= x.EndDate.Date)
                                      select a;

            List<ApplTask> applTaskDelegate = [];
            var elements = conn.GetList<ApplTask>();
            foreach (var applTaskDelegation in applTaskDelegations)
            {
                var child = elements.First(x => x.Id == applTaskDelegation.ApplTaskId);
                applTaskDelegate.Add(child);
                var parents = FindAllParents(elements.ToList(), child).ToList();
                foreach (var item in parents)
                {
                    var applTasksExist = applTaskDelegate.Where(x => x.Id == item.Id);
                    if (!applTasksExist.Any())
                        applTaskDelegate.Add(item);
                }
            }
            IEnumerable<ApplTask> enumerable = applTasks.Concat(applTaskDelegate);
            return await Task.FromResult(enumerable);
        }

    }
}