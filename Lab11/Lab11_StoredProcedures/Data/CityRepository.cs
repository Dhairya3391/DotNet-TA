using Lab11_StoredProcedures.Models;
using System.Data;

namespace Lab11_StoredProcedures.Data
{
    public class CityRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public CityRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Get all cities using PR_City_SelectAll
        public List<City> GetAllCities()
        {
            List<City> cities = new List<City>();
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_City_SelectAll");

            foreach (DataRow row in dt.Rows)
            {
                cities.Add(new City
                {
                    CityID = Convert.ToInt32(row["CityID"]),
                    CityName = row["CityName"].ToString(),
                    CityCode = row["CityCode"].ToString(),
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                });
            }

            return cities;
        }

        // Get city by ID using PR_City_SelectByPK
        public City? GetCityById(int cityId)
        {
            var parameter = _dbHelper.CreateParameter("@CityID", cityId, System.Data.SqlDbType.Int);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_City_SelectByPK", parameter);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new City
                {
                    CityID = Convert.ToInt32(row["CityID"]),
                    CityName = row["CityName"].ToString(),
                    CityCode = row["CityCode"].ToString(),
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                };
            }

            return null;
        }

        // Filter cities by name using PR_City_SelectByName (Lab 11 specific)
        public List<City> GetCitiesByName(string cityName)
        {
            List<City> cities = new List<City>();
            var parameter = _dbHelper.CreateParameter("@CityName", cityName, System.Data.SqlDbType.NVarChar);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_City_SelectByName", parameter);

            foreach (DataRow row in dt.Rows)
            {
                cities.Add(new City
                {
                    CityID = Convert.ToInt32(row["CityID"]),
                    CityName = row["CityName"].ToString(),
                    CityCode = row["CityCode"].ToString(),
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                });
            }

            return cities;
        }

        // Get cities by state using PR_City_SelectByState (Lab 11 specific)
        public List<City> GetCitiesByState(int stateId)
        {
            List<City> cities = new List<City>();
            var parameter = _dbHelper.CreateParameter("@StateID", stateId, System.Data.SqlDbType.Int);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_City_SelectByState", parameter);

            foreach (DataRow row in dt.Rows)
            {
                cities.Add(new City
                {
                    CityID = Convert.ToInt32(row["CityID"]),
                    CityName = row["CityName"].ToString(),
                    CityCode = row["CityCode"].ToString(),
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString(),
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                });
            }

            return cities;
        }
    }
}