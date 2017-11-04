using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Fchat.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace Fchat.Hubs
{
    public class Chat : Hub
    {

        private MTravelEntities db = new MTravelEntities();

        private static readonly List<OnlineUser> Users = new List<OnlineUser>();


        //public void SendMsgToPerson(string reciveID, string msg)
        //{
        //    var sentID = Context.ConnectionId;
        //    var classRecive = "recive";
        //    var classSend = "send";
        //    Clients.Client(reciveID).addNewMessageToPage(classRecive, msg);
        //    Clients.Client(sentID).addNewMessageToPage(classSend, msg);
        //}


        public void sentfkingmessage (string reciveID , string msg)
        {
            var sentID = Context.ConnectionId;
            var classRecive = "recive";
            var classSend = "send";
            Clients.Client(reciveID).addNewMessageToPage(classRecive, msg);
            Clients.Client(sentID).addNewMessageToPage(classSend, msg);
        }


        //[HubMethodName("john")]
        public void Join(int id)
        {

            var user = db.Users.SingleOrDefault(x => x.IDuser == id);
            // Add the new user

            var role = db.CTQs.SingleOrDefault(x => x.IDuser == id);

            if(role.IdQuyen == 2)
            {
                
                if(Users.SingleOrDefault(x=>x.name == user.HoTen) == null)
                {
                    Users.Add(new OnlineUser
                    {
                        name = user.HoTen,
                        connectionID = Context.ConnectionId
                    });

                }
            }

            // Send down the new list to all clients
            SendUserListUpdate();
        }

        public void SendUserListUpdate()
        {
            //List<OnlineUser> availableConnect = new List<OnlineUser>();
            //availableConnect = userList.Where(x => x.isConnect == false).ToList();
            //Clients.All.upadateUsersList(availableConnect);
            
            Clients.All.updateUserList(Users);
        }

        public void connectToPersion(string connectionID)
        {
            var from = Context.ConnectionId;
            var to = connectionID;

            var isConnected = Users.SingleOrDefault(x => x.connectionID == connectionID).isConnect;

            if (isConnected)
            {
                Clients.Client(from).ReciveAMessage("Người dùng hiện đang trong một cuộc test");
            }
            else
            {
                Clients.Client(to).reciveMessageOnConnect(from);

            }

        }

        public void Accept (string connectionID)
        {
            var from = Context.ConnectionId;
            var data = Users.SingleOrDefault(x => x.connectionID == connectionID);
            data.isConnect = true;



            Clients.Client(connectionID).AcceptReq(data);
        }


        public void Deny(string connectionID)
        {
            var from = Context.ConnectionId;
            var to = connectionID;

            Clients.Client(to).ReciveAMessage("Người dùng từ chối cuộc gọi");
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            Users.RemoveAll(x=>x.connectionID == Context.ConnectionId);

                        SendUserListUpdate();


            return base.OnDisconnected(stopCalled);
        }
        

        //public override System.Threading.Tasks.Task OnDisconnected()
        //{
        //    // Hang up any calls the user is in
        //    HangUp(); // Gets the user from "Context" which is available in the whole hub

        //    // Remove the user
        //    Users.RemoveAll(u => u.ConnectionId == Context.ConnectionId);

        //    // Send down the new user list to all clients
        //    SendUserListUpdate();

        //    return base.OnDisconnected();
        //}
    }
}