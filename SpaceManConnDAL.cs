using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SpaceManConnectedLayer
{
    public class SpaceManDAL : IDisposable
    {
        private SqlConnection SqlCn { get; set; }

        public void OpenConnection(string connectionString)
        {
            SqlCn = new SqlConnection { ConnectionString = connectionString };
            SqlCn.Open();
        }

        public void CloseConnection()
        {
            SqlCn.Close();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        // management plan
        public void Insert(ManagementPlan planObj)
        {
            string sqlStr = string.Format("Insert Into ManagementPlan (MP_Name, MP_Company, MP_Address) Values" +
                                         "('{0}', '{1}', '{2}')",
                                         planObj.Name, planObj.Company, planObj.Address);
            using (SqlCommand cmd = new SqlCommand(sqlStr, SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't insert the Management Plan! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public void DeleteManagementPlan(int id)
        {
            string sqlStr = string.Format("Delete from ManagementPlan where MP_ID = '{0}'", id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't delete the Management Plan! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public void Update(ManagementPlan planObj, int id)
        {
            string sqlStr =
       string.Format(
           "Update ManagementPlan Set MP_Name = '{0}', MP_Company = '{1}', MP_Address = '{2}' Where MP_ID = '{3}'",
           planObj.Name, planObj.Company, planObj.Address, id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't update the Management Plan! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public DataTable GetManagementPlan(int id)
        {
            DataTable mp = new DataTable();
            string sqlStr = string.Format("Select * from ManagementPlan where MP_ID = {0}", id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader();
                    mp.Load(dr);
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get Management Plan Data! " + ex.Message, ex);
                    throw error;
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            return mp;
        }

        public DataTable GetAllManagementPlans()
        {
            DataTable plans = new DataTable();
            string sqlStr = string.Format("Select * From ManagementPlan");

            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader();
                    plans.Load(dr);
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get Management Plans! " + ex.Message, ex);
                    throw error;
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            return plans;
        }

        public int? GetManagementPlanId(string mpName)
        {
            int? tmp = null;
            string sqlStr = String.Format("Select MP_ID from ManagementPlan where MP_Name = '{0}'", mpName);
            using (SqlCommand cmd = new SqlCommand(sqlStr, SqlCn))
            {
                // execute sql statement and get MP_ID
                try
                {
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            tmp = dataReader["MP_ID"] as int?;
                        }

                    }
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get the Management Plan Id! " + ex.Message, ex);
                    throw error;
                }
            }
            return tmp;
        }

        // room
        public void Insert(Room roomObj, int id)
        {
            string sqlStr = string.Format("Insert Into Room" +
      "(R_PetName, R_SeatsNumber, R_StandsNumber, R_Storey, R_HasProjector, R_Description, R_MP_ID, R_AddedTime) Values" +
      "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')"
      , roomObj.PetName, roomObj.SeatsNumber,
      roomObj.StandsNumber, roomObj.Storey, roomObj.HasProjector, roomObj.Description, id, DateTime.Now);

            // execute sql statement and insert roomObj data
            using (SqlCommand cmd = new SqlCommand(sqlStr, SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't insert the Room! " + ex.Message, ex);
                    throw error;
                }

            }
        }

        public bool InsertRoom(string name, short seats, short stands, short storey, bool projector, string description, int mpID)
        {
            //var tmpName = GetManagementPlanId(mpName);
            //if (tmpName == null)
            //    return false;

            string sqlStr = string.Format("Insert Into Room" +
                                       "(R_PetName, R_SeatsNumber, R_StandsNumber, R_Storey, R_HasProjector, R_Description, R_MP_ID, R_AddedTime) Values" +
                                       "(@Name, @Seats, @Stands, @Storey, @Projector, @Description, @MP_ID, @Date)");


            // execute sql statement and insert roomObj data
            using (SqlCommand cmd = new SqlCommand(sqlStr, SqlCn))
            {
                // fill in parameters collection
                SqlParameter p1 = new SqlParameter("@Name", SqlDbType.NVarChar, 10);
                p1.Value = name;

                SqlParameter p2 = new SqlParameter("@Seats", SqlDbType.SmallInt);
                p2.Value = seats;
                SqlParameter p3 = new SqlParameter("@Stands", SqlDbType.SmallInt);
                p3.Value = stands;

                SqlParameter p4 = new SqlParameter("@Storey", SqlDbType.SmallInt);
                p4.Value = storey;

                SqlParameter p5 = new SqlParameter("@Projector", SqlDbType.Bit);
                p5.Value = projector;

                SqlParameter p6 = new SqlParameter("@Description", SqlDbType.NVarChar, 200);
                p6.Value = description;

                SqlParameter p7 = new SqlParameter("@MP_ID", SqlDbType.Int);
                p7.Value = mpID;

                SqlParameter p8 = new SqlParameter("@Date", SqlDbType.DateTime);
                p8.Value = DateTime.Now;

                cmd.Parameters.AddRange(new SqlParameter[] { p1, p2, p3, p4, p5, p6, p7, p8 });

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't insert the Room! " + ex.Message, ex);
                    throw error;
                }

            }
            return true;
        }

        public void DeleteRoom(int id)
        {
            string sqlStr = string.Format("Delete from Room where R_ID = '{0}'", id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't delete the Room! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public void Update(Room roomObj, int id)
        {
            string sqlStr =
    string.Format(
        "Update Room Set R_PetName = '{0}', R_SeatsNumber = '{1}', R_StandsNumber = '{2}', R_Storey = '{3}', R_HasProjector = '{4}', R_Description = '{5}' Where R_ID = '{6}'",
        roomObj.PetName, roomObj.SeatsNumber, roomObj.StandsNumber, roomObj.Storey, roomObj.HasProjector, roomObj.Description,
        id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Exception error = new Exception("Couldn't update the Room! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public DataTable GetRooms(int id)
        {
            DataTable rooms = new DataTable();

            string sqlStr = string.Format("Select * From Room where R_MP_ID = {0}", id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader();
                    rooms.Load(dr);
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get the Room Data Table! " + ex.Message, ex);
                    throw error;
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            return rooms;
        }

        public List<Room> GetAllRoomsAsList()
        {
            List<Room> rooms = new List<Room>();

            string sqlStr = "Select * From Room";
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        rooms.Add(new Room
                        {
                            ID = (int)dr["R_ID"],
                            PetName = (string)dr["R_PetName"],
                            SeatsNumber = (short)dr["R_SeatsNumber"],
                            StandsNumber = (short)dr["R_StandsNumber"],
                            Storey = (byte)dr["R_Storey"],
                            HasProjector = (bool)dr["R_HasProjector"],
                            Description = (string)dr["R_Description"],
                            AddedTime = (DateTime)dr["R_AddedTime"],
                            MP_ID = (int)dr["R_MP_ID"]
                        });
                    }
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get the Room List! " + ex.Message, ex);
                    throw error;
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }

            }
            return rooms;
        }

        public string LookUpRoomPetName(int roomID)
        {
            string roomPetName = string.Empty;

            using (SqlCommand cmd = new SqlCommand("GetRoomPetName", this.SqlCn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // input parameters
                SqlParameter p = new SqlParameter("@ID", SqlDbType.Int);
                p.Value = roomID;
                p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p);

                // output parameters
                p = new SqlParameter("@PetName", SqlDbType.NVarChar, 10);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);

                // execute stored procedure
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't look up the Room name! " + ex.Message, ex);
                    throw error;
                }

                // return output parameter
                roomPetName = (string)cmd.Parameters["@PetName"].Value;
            }
            return roomPetName;
        }

        // reservation
        public void Insert(Reservation rsvObj, int id)
        {
            string sqlStr =
               string.Format(
                   "Insert into Reservation (RV_R_ID, RV_Person, RV_Date, RV_TimeFrom, RV_TimeTo) Values" +
                   "('{0}', '{1}', '{2}', '{3}', '{4}')", id, rsvObj.Person, rsvObj.Date, rsvObj.TimeFrom, rsvObj.TimeTo);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't insert the Reservation! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public void DeleteReservation(int id)
        {
            string sqlStr = string.Format("Delete from Reservation where RV_ID = '{0}'", id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't delete the Reservation!", ex);
                    throw error;
                }
            }
        }

        public void Update(Reservation rsvObj, int id)
        {
            string sqlStr =
           string.Format(
               "Update Reservation Set RV_Person = '{0}', RV_TimeFrom = '{1}', RV_TimeTo = '{2}' Where RV_ID = '{3}'",
               rsvObj.Person, rsvObj.TimeFrom, rsvObj.TimeTo, id);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't update the Reservation! " + ex.Message, ex);
                    throw error;
                }
            }
        }

        public DataTable GetAllReservations(int mpID, DateTime day)
        {
            DataTable reservations = new DataTable();

            string sqlStr =
                string.Format(
                    "Select RV_ID, RV_R_ID, R_PetName, RV_Person, RV_Date, RV_TimeFrom, RV_TimeTo From Reservation Inner Join Room on RV_R_ID = R_ID Where R_MP_ID = '{0}' and RV_Date = '{1}'",
                    mpID, day);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader();
                    reservations.Load(dr);
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get the Reservations! " + ex.Message, ex);
                    throw error;
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            return reservations;
        }

        public DataTable GetSingleRoomReservations(int mpID, int roomID, DateTime day)
        {
            DataTable reservations = new DataTable();

            string sqlStr =
                string.Format(
                    "Select RV_ID, R_PetName, RV_Person, RV_Date, RV_TimeFrom, RV_TimeTo From Reservation Inner Join Room on RV_R_ID = R_ID Where RV_Date = '{0}' and R_MP_ID = '{1}' and R_ID = '{2}'",
                    day, mpID, roomID);
            using (SqlCommand cmd = new SqlCommand(sqlStr, this.SqlCn))
            {
                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader();
                    reservations.Load(dr);
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Couldn't get the single roomObj Reservations! " + ex.Message, ex);
                    throw error;
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            return reservations;
        }

    }
}
