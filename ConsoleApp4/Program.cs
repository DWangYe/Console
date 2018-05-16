using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Person> personList = new List<Person>()
            //{
            //    new Person(){ID=1,Name="Big Yellow",Age=10},
            //    new Person(){ID=2,Name="Little White",Age=15},
            //    new Person(){ID=3,Name="Middle Blue",Age=-7}
            //};

            // 下面就使用了IEnumerable的扩展方法：Where
            //var datas = personList.Where(delegate (Person p)
            //{
            //    int i = 10;
            //    return p.Age >= i;
            //});

            //var datas = personList.Where(p => p.Age >= 10);
            //var datas = personList;
            //foreach (var data in datas)
            //{
            //    Console.WriteLine("ID:{0},姓名：{1},年龄：{2}",
            //        data.ID, data.Name, data.Age);
            //}
            //Stack<string> ss = new Stack<string>();
            //for (int i = 0; i < 10; i++)
            //{
            //    ss.Push(""+i);
            //}
            //Age a = new Age() { Num = -1 };
            //Age b = new Age() { Num = 2 };

            //Console.WriteLine(a.Num + b.Num);

            //Console.WriteLine(a + b);
            //Console.WriteLine(a - b);

            //a++;
            //Console.WriteLine(a);


            //Console.WriteLine(a.IsYouth);
            //Console.ReadKey();

            string url = "http://180.167.88.130:38080/xinbox/toApplyCredit_inst?";

            string param = string.Format("apply_name={0}&apply_idnum={1}&apply_phone={2}&apply_type=1&operCode={3}&orgno={4}&platform={5}", "周瑜", "410101190101011234", "18987654321", 8888888, 88888, 88);
            string param2 = string.Format("apply_name={0}&apply_idnum={1}&apply_phone={2}&operCode={3}&orgno={4}&platform={5}", "周瑜", "410101190101011234", "18987654321", 8888888, 88888, 88);
            string ss = param2 + "&key=Qwer123";

            string sign = MD5Encrypt(ss);

            Console.WriteLine(url + param + "&sign=" + sign);
            Console.ReadKey();

        }
        public static string MD5Encrypt(string encryptString)
        {
            if (string.IsNullOrEmpty(encryptString))
            {
                throw (new Exception("密文不得为空"));
            }
            MD5 m_ClassMD5 = new MD5CryptoServiceProvider();
            string m_strEncrypt = "";
            try
            {
                m_strEncrypt = BitConverter.ToString(m_ClassMD5.ComputeHash(Encoding.UTF8.GetBytes(encryptString))).Replace("-", "");
            }
            catch (ArgumentException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_ClassMD5.Clear(); }
            return m_strEncrypt;
        }
        //public static IEnumerable<int> Add()
        //{
        //        for (int i = 0; i < 10; i++)
        //        {
        //            yield return i;
        //        }
        //    yield break;
        //}
    }

    public class Age
    {
        #region 字段 存储数据与外界隔离
        private int _num;
        #endregion

        #region 属性 连接外界与字段
        public int Num
        {
            get => _num;
            set
            {
                _num = value;
                if (value < 0)
                {
                    _num = 0;
                }
            }
        }

        public bool IsYouth
        {
            get
            {
                if (_num<16)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region 方法


        #region 重载后的方法
        public static int operator +(Age a, Age b)
        {
            return a.Num + b.Num;
        }
        public static int operator -(Age a, Age b)
        {
            return a.Num - b.Num;
        }
        public static Age operator ++(Age a)
        {
            a.Num++;
            return a;
        }
        public override string ToString()
        {
            return Num.ToString();
        }
        #endregion

        #endregion
    }



}
