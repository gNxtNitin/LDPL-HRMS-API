using DatabaseManager;
using NotificationPreferenceLib.Interface;
using NotificationPreferenceLib.Models;
using System.Collections;
using System.Data;

namespace NotificationPreferenceLib
{
    public class NotifcationPreferenceService : INotificationPreferenceService
    {
       

        public async Task<NotificationPreference> GetNotificationPreferenceByIdAsync(int npid)
        {
            NotificationPreference preference = null;
            ArrayList arrList = new ArrayList();

            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "I", "CHAR", "I"); 
                DAL.spArgumentsCollection(arrList, "@NPID", npid.ToString(), "TINYINT", "I"); 

                DataSet ds = new DataSet();
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteNotificationPreferences", arrList);


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    preference = new NotificationPreference
                    {
                        NPID = Convert.ToInt32(ds.Tables[0].Rows[0]["NPID"]),
                        Preference = ds.Tables[0].Rows[0]["Preference"].ToString()
                    };
                }
                else
                {
                    Console.WriteLine("No notification preference found for the given NPID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            //return preference; 
            return await Task.FromResult(preference);
        }

        //Get all notification preferences.
        public async Task<List<NotificationPreference>> GetAllNotificationPreferencesAsync()
        {
            List<NotificationPreference> preferences = new List<NotificationPreference>();
            ArrayList arrList = new ArrayList();

            try
            {
                
                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I");

                DataSet ds = new DataSet();
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteNotificationPreferences", arrList);

                //DataSet ds = await Task.Run(() => DAL.RunStoredProcedure(new DataSet(), "sp_GetSetDeleteNotificationPreferences", arrList));


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        preferences.Add(new NotificationPreference
                        {
                            NPID = Convert.ToInt32(row["NPID"]),
                            Preference = row["Preference"].ToString(),
                        });
                    }
                }
                else
                {
                    
                    Console.WriteLine("No notification preferences found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

          
            return await Task.FromResult(preferences);
        }




        // Create or Update Notification Preferences
        public async Task<bool> SaveOrUpdateNotificationPreferenceAsync(NotificationPreference preference, bool isCreate)
        {
            ArrayList arrList = new ArrayList(); 

            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", isCreate ? "C" : "U", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@NPID", preference.NPID.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Preference", preference.Preference, "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@CreatedBy", preference.CreatedBy.ToString(), "INT", "I");

                
                await Task.Run(() => DAL.RunStoredProcedure("sp_GetSetDeleteNotificationPreferences", arrList));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }




        // Delete Notification Preference by NPID
        public async Task<bool> DeleteNotificationPreferenceAsync(int npid)
        {
            ArrayList arrList = new ArrayList(); 

            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "D", "CHAR", "I"); 
                DAL.spArgumentsCollection(arrList, "@NPID", npid.ToString(), "INT", "I"); 

                await Task.Run(() => DAL.RunStoredProcedure("sp_GetSetDeleteNotificationPreferences", arrList));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        // Get User Notification Preferences by UNPID (Specific user preference)

   
        public async Task<List<UserNotificationPreference>> GetUserNotificationPreferenceByIdAsync(int unpid)
        {
            UserNotificationPreference userPreference = null;
            List<UserNotificationPreference> unpList = new List<UserNotificationPreference>();
            ArrayList arrList = new ArrayList();

            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "I", "CHAR", "I");
               
                DAL.spArgumentsCollection(arrList, "@UserID", unpid.ToString(), "INT", "I");

                DataSet ds = new DataSet();
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteUserNotifyPreferences", arrList);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];

                        userPreference = new UserNotificationPreference
                        {
                            UNPID = Convert.ToInt32(row["UNPID"]),
                            NPID = Convert.ToInt32(row["NPID"]),
                            UserId = Convert.ToInt32(row["UserID"]),

                            Preference = new NotificationPreference
                            {
                                NPID = Convert.ToInt32(row["NPID"]),
                                Preference = row["Preference"].ToString(),

                            }
                        };
                        unpList.Add(userPreference);
                    }
                }
                else
                {
                    Console.WriteLine("No user notification preference found for the given UNPID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            // return userPreference;
            return await Task.FromResult(unpList);
        }



        // Get All User Notification Preferences
        public async Task<List<UserNotificationPreference>> GetAllUserNotificationPreferencesAsync()
        {
            List<UserNotificationPreference> userPreferences = new List<UserNotificationPreference>();
            ArrayList arrList = new ArrayList(); 

            try
            {
               
                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I"); 

                
                DataSet ds = await Task.Run(() => DAL.RunStoredProcedure(new DataSet(), "sp_GetSetDeleteUserNotifyPreferences", arrList));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        userPreferences.Add(new UserNotificationPreference
                        {
                            UNPID = Convert.ToInt32(row["UNPID"]),
                            NPID = Convert.ToInt32(row["NPID"]),
                            UserId = Convert.ToInt32(row["UserID"]),
                          
                            Preference = new NotificationPreference
                            {
                                NPID = Convert.ToInt32(row["NPID"]),
                                Preference = row["Preference"].ToString(),
                              
                            }
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No user notification preferences found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return userPreferences; 
        }



        // Create or Update User Notification Preferences
        public async Task<bool> SaveOrUpdateUserNotificationPreferenceAsync(UserNotificationPreference userPreference, bool isCreate)
        {
            ArrayList arrList = new ArrayList(); 

            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", isCreate ? "C" : "U", "CHAR", "I"); 
                DAL.spArgumentsCollection(arrList, "@UNPID", userPreference.UNPID.ToString(), "INT", "I"); 
                DAL.spArgumentsCollection(arrList, "@NPID", userPreference.NPID.ToString(), "TINYINT", "I"); 
                DAL.spArgumentsCollection(arrList, "@UserID", userPreference.UserId.ToString(), "INT", "I"); 
                DAL.spArgumentsCollection(arrList, "@CreatedBy", userPreference.CreatedBy.ToString(), "INT", "I"); 

                await Task.Run(() => DAL.RunStoredProcedure(new DataSet(), "sp_GetSetDeleteUserNotifyPreferences", arrList));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SaveOrUpdateUserNotificationPreference: " + ex.Message);
                return false;
            }
        }


        // Delete User Notification Preference by UNPID
        public async Task<bool> DeleteUserNotificationPreferenceAsync(int unpid)
        {
            ArrayList arrList = new ArrayList();

            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UNPID", unpid.ToString(), "INT", "I");


                //DataSet ds = new DataSet();
                //ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteUserNotifyPreferences", arrList);

                await Task.Run(() => DAL.RunStoredProcedure("sp_GetSetDeleteUserNotifyPreferences", arrList));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteUserNotificationPreference: " + ex.Message);
                return false;
            }
        }


      
    }
}
