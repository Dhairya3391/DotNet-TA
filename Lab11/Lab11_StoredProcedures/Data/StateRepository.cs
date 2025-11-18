using Lab11_StoredProcedures.Models;
using System.Data;

namespace Lab11_StoredProcedures.Data
{
    public class StateRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public StateRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Get all states using PR_State_SelectAll
        public List<State> GetAllStates()
        {
            List<State> states = new List<State>();
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_State_SelectAll");

            foreach (DataRow row in dt.Rows)
            {
                states.Add(new State
                {
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    StateCode = row["StateCode"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                });
            }

            return states;
        }

        // Get state by ID using PR_State_SelectByPK
        public State? GetStateById(int stateId)
        {
            var parameter = _dbHelper.CreateParameter("@StateID", stateId, System.Data.SqlDbType.Int);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_State_SelectByPK", parameter);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new State
                {
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    StateCode = row["StateCode"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                };
            }

            return null;
        }

        // Get states with city count using PR_State_SelectWithCityCount (Lab 11 specific)
        public List<State> GetStatesWithCityCount()
        {
            List<State> states = new List<State>();
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_State_SelectWithCityCount");

            foreach (DataRow row in dt.Rows)
            {
                states.Add(new State
                {
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    StateCode = row["StateCode"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString(),
                    CityCount = Convert.ToInt32(row["CityCount"])
                });
            }

            return states;
        }
    }
}