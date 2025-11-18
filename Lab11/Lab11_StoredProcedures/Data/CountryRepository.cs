using Lab11_StoredProcedures.Models;
using System.Data;

namespace Lab11_StoredProcedures.Data
{
    public class CountryRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public CountryRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Get all countries using PR_Country_SelectAll
        public List<Country> GetAllCountries()
        {
            List<Country> countries = new List<Country>();
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_Country_SelectAll");

            foreach (DataRow row in dt.Rows)
            {
                countries.Add(new Country
                {
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString(),
                    CountryCode = row["CountryCode"].ToString()
                });
            }

            return countries;
        }

        // Get country by ID using PR_Country_SelectByPK
        public Country? GetCountryById(int countryId)
        {
            var parameter = _dbHelper.CreateParameter("@CountryID", countryId, System.Data.SqlDbType.Int);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_Country_SelectByPK", parameter);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Country
                {
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString(),
                    CountryCode = row["CountryCode"].ToString()
                };
            }

            return null;
        }
    }
}