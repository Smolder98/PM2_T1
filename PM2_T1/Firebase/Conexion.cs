using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2_T1.Firebase
{
    public class Conexion
    {
        public static FirebaseClient firebase = new FirebaseClient("https://pm2p3-t1-default-rtdb.firebaseio.com/");
    }
}
