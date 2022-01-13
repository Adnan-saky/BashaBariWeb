using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TBashaBari.Controllers
{
    public class DatabaseConnection
    {
        SqlCommand queryString = new SqlCommand();
        SqlDataReader datareader;
        SqlConnection conn = new SqlConnection();
        public void DbConnect()
        {
            /*
              Actual string looks like this: 
                             Dinar :-  "Data Source = \"DESKTOP-RUB62SE\\SQLEXPRESS\"; database = \"BashaBariWeb\"; integrated security = SSPI;";
                             saky  :-  "Data Source = \"DESKTOP-37APNEN\\SQLEXPRESS\"; database = \"BashaBariWeb\"; integrated security = SSPI;";
                             Sajid :-  "Data Source = \"DESKTOP-42FFPT6\\SQLEXPRESS\"; database = \"BashaBariWeb\"; integrated security = SSPI;";
             */
            conn.ConnectionString = "Data Source = \"DESKTOP-42FFPT6\\SQLEXPRESS\"; database = \"BashaBariWeb\"; integrated security = SSPI;";
            conn.Open();
            queryString.Connection = conn;

        }

        public SqlDataReader ExeQuery(string queryString)
        {
            try
            {
                this.queryString.CommandText = queryString;
                datareader = this.queryString.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return datareader;
        }

        public string getUserFullName(string userEmail)
        {
            try
            {
                DbConnect();
                queryString.CommandType = CommandType.Text;
                queryString.Parameters.Add("FullName", SqlDbType.VarChar).Value = userEmail;
                queryString.CommandText = "SELECT [FullName] FROM [BashaBariWeb].[dbo].[AspNetUsers] WHERE [UserName] = '" + userEmail + "'";
                string userFullName = queryString.ExecuteScalar().ToString();
                CloseDbConnect();

                return userFullName;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public string getTenantsOwner(string tenantEmail)
        {
            try
            {
                DbConnect();
                queryString.CommandType = CommandType.Text;
                queryString.Parameters.Add("FullName", SqlDbType.VarChar).Value = tenantEmail;
                queryString.CommandText = "SELECT [OwnerEmail] FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] WHERE [TenantEmail] = '" + tenantEmail + "'";

                if (queryString.ExecuteScalar() != null)
                {
                    string ownerEmail = queryString.ExecuteScalar().ToString();

                    CloseDbConnect();

                    return ownerEmail;
                }
                else {
                    CloseDbConnect();
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool isBillInformationExist(string tenantEmail, string billTime)
        {
            DbConnect();
            queryString.CommandType = CommandType.Text;
            queryString.CommandText = "SELECT[TenantEmail] FROM [BashaBariWeb].[dbo].[BillInformation] WHERE [BillTime] = '" + billTime + "' AND [TenantEmail] = '" + tenantEmail + "'";

            string tempstr = "";
            if (queryString.ExecuteScalar() != null)
            {
                tempstr = queryString.ExecuteScalar().ToString();
            }
            CloseDbConnect();

            bool booltemp = false;

            if (tenantEmail.Equals(tempstr.Trim()))
            {
                booltemp = true;
            }
            return booltemp;
        }

        public bool isTenantConnected(string tenantEmail)
        {
            DbConnect();
            queryString.CommandType = CommandType.Text;
            queryString.CommandText = "SELECT [TenantEmail] FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] WHERE [TenantEmail] = '" + tenantEmail + "' AND [IsConfirmed] = 'Yes'";

            string tempstr = "";
            if (queryString.ExecuteScalar() != null)
            {
                tempstr = queryString.ExecuteScalar().ToString();
            }
            CloseDbConnect();

            bool booltemp = false;

            if (tenantEmail.Equals(tempstr.Trim()))
            {
                booltemp = true;
            }
            return booltemp;
        }

        public bool isOwnerConnected(string ownerEmail)
        {
            DbConnect();
            queryString.CommandType = CommandType.Text;
            queryString.CommandText = "SELECT TOP 1 [OwnerEmail] FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] WHERE [OwnerEmail] = '" + ownerEmail + "' AND [IsConfirmed] = 'Yes'";

            string tempstr = "";
            if (queryString.ExecuteScalar() != null)
            {
                tempstr = queryString.ExecuteScalar().ToString();
            }
            CloseDbConnect();

            bool booltemp = false;

            if (ownerEmail.Equals(tempstr.Trim()))
            {
                booltemp = true;
            }
            return booltemp;
        }

        public bool isEmailConfirmed(string _Email)
        {
            DbConnect();
            queryString.CommandType = CommandType.Text;
            queryString.CommandText = "SELECT [Email] FROM [BashaBariWeb].[dbo].[AspNetUsers] WHERE [Email] = '" + _Email + "' AND [EmailConfirmed] = '1'";

            string tempstr = "";
            if (queryString.ExecuteScalar() != null)
            {
                tempstr = queryString.ExecuteScalar().ToString();
            }
            CloseDbConnect();

            bool booltemp = false;

            if (_Email.Equals(tempstr.Trim()))
            {
                booltemp = true;
            }
            return booltemp;
        }
        public string getUserPhoneNumber(string userEmail)
        {
            try
            {
                DbConnect();
                queryString.CommandType = CommandType.Text;
                queryString.Parameters.Add("PhoneNumber", SqlDbType.VarChar).Value = userEmail;
                queryString.CommandText = "SELECT [PhoneNumber] FROM [BashaBariWeb].[dbo].[AspNetUsers] WHERE [UserName] = '" + userEmail + "'";
                string userPhoneNumber = queryString.ExecuteScalar().ToString();
                CloseDbConnect();

                return userPhoneNumber;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public void CloseDbConnect()
        {
            conn.Close();
        }
    }
}
