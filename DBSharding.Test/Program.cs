using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBSharding.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //注册数据库连接
            IDbConnection db0 = new MySqlConnection(ConfigurationManager.ConnectionStrings["db0"].ConnectionString);
            IDbConnection db1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["db1"].ConnectionString);
            IDbConnection db2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["db2"].ConnectionString);

            Dictionary<string, IDbConnection> connectionDic = new Dictionary<string, IDbConnection>();
            connectionDic.Add("0", db0);
            connectionDic.Add("1", db1);
            connectionDic.Add("2", db2);

            ShardingConnUtils.RegisConnGroup(connectionDic);



            Enumerable.Range(1, 500).ToList().ForEach(a =>
            {
                //测试
                User user = new User();
                user.Id = a;
                user.UserName = "name_" + a;

                UserRepertory userRepertory = new UserRepertory();
                User user2 = userRepertory.GetUserById(a);
                if (user2 == null)
                {
                    userRepertory.AddUser(user);
                }
                else
                {
                    Console.WriteLine(user2.UserName+"已存在！");
                }
            
            });
         

            Console.ReadKey();

        }
    }
}
