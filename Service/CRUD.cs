using BasketballWebApp.Models;
using BasketballWebApp.Service;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketballBusinessLayer
{
    public class CRUD : ICRUD
    {
        public UserTeams SelectedUserTeam { get; set; }
        public Nbateams SelectedNbaTeam { get; set; }
        public Players SelectedPlayers { get; set; }
        public Users SelectedUser { get; set; }

        private BasketballProjectContext _context;

        public CRUD(BasketballProjectContext context)
        {
            _context = context;
        }

        public IOrderedQueryable<Nbateams> RetrieveNbaTeams()
        {
            var teams = _context.Nbateams.OrderByDescending(t => t.Wins);
            return teams;
        }

        public IOrderedQueryable<Players> RetrieveTeamPlayers(int? id)
        {
            var players = _context.Players.Where(p => p.TeamId == id).OrderByDescending(p => p.Price);
            return players;
        }

        public async Task<Players> RetrievePlayerDetails(int? id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            return player;
        }

        public IQueryable<UserTeams> AllUserTeams()
        {
            var fantasyTeam = _context.UserTeams.Include(u => u.User).Where(ut => ut.UserId == 1);
            return fantasyTeam;
        }

        public IQueryable<UserTeamPlayers> RetrieveUserTeamsPlayers(int? id)
        {
            //var fantasyPlayers = _context.UserTeamPlayers
            //                .Include(u => u.Player).Include(u => u.UserTeam)
            //                .Where(ut => ut.UserTeamId == id);

            SelectedUserTeam = _context.UserTeams.Where(ut => ut.UserTeamId == id).FirstOrDefault();
            var fantasyPlayers = from uTeamPlayers in _context.UserTeamPlayers.Include(ut => ut.UserTeam).Include(p => p.Player)
                                 where (uTeamPlayers.UserTeamId == id)
                                 select uTeamPlayers;

            return fantasyPlayers;
        }

        public async Task<UserTeams> RetrieveUserTeamsDetails(int? id)
        {
            var teamDetails = await _context.UserTeams.FirstOrDefaultAsync(ut => ut.UserTeamId == id);
            return teamDetails;
        }

        public async Task RemoveUserTeam(int? id)
        {
            var userTeam = await _context.UserTeams.FindAsync(id);
            _context.UserTeams.Remove(userTeam);
            await _context.SaveChangesAsync();
            ////Find and Delete the User Team players in the UserTeamPlayers database
            //var userTeamPlayers = _context.UserTeamPlayers.Where(ut => ut.UserTeamId == id);
            //_context.UserTeamPlayers.RemoveRange(userTeamPlayers);
            // await _context.SaveChangesAsync();


            ////Find and Delete the User Team
            //var userTeam = await _context.UserTeams.Where(ut=>ut.UserTeamId == id).FirstOrDefaultAsync();
            //_context.Remove<UserTeams>(userTeam);
            //await _context.SaveChangesAsync();
        }


        public async Task CreateNewTeam(UserTeams userTeam)
        {
            _context.Add(userTeam);
            await _context.SaveChangesAsync();
        }

        public async Task AddPlayer(int? id)
        {
            var player = _context.Players.Where(p => p.PlayerId == id).FirstOrDefault();


            var numberOfPlayersInTeam = RetrieveUserTeamsPlayers(SelectedUserTeam.UserTeamId).Count(); // Get all players in team
            //Check if the player is in the team
            var searchForPlayerInTeam = _context.UserTeamPlayers.Where(ut => ut.UserTeamId == SelectedUserTeam.UserTeamId).Where(ut => ut.PlayerId == id);
            var isPlayerAlreadyInTeam = searchForPlayerInTeam.Count();

            //Ensure the player has less than 6 players in their team and then the player is in budget
            if (isPlayerAlreadyInTeam != 1)
            {
                if (numberOfPlayersInTeam < 6)
                    if (CheckBudget() == true)
                    {
                        var newPlayer = new UserTeamPlayers { PlayerId = (int)id, UserTeamId = SelectedUserTeam.UserTeamId };
                        //Update budget and then add the player
                        SelectedUserTeam.Budget -= player.Price;
                        _context.UserTeams.Update(SelectedUserTeam);
                        await _context.SaveChangesAsync();

                        _context.UserTeamPlayers.Add(newPlayer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new OutOfBudgetException();
                    }
                else
                {
                    throw new TooManyPlayerException();
                }
            }
        }

        public bool CheckBudget()
        {
            if (SelectedPlayers.Price <= SelectedUserTeam.Budget)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetUserTeamID(int? id)
        {
            return (int)id;
        }

        public IEnumerable<Players> RetrievePlayers()
        {
            return _context.Players.ToList();
        }

        public IEnumerable<UserTeams> RetrieveUserTeam(int? id)
        {
            return _context.UserTeams.Where(ut => ut.UserTeamId == id).ToList();
        }
    }
}

//        public decimal ResetBudget()
//        {
//            using (var db = new BasketballProjectContext())
//            {
//                SelectedUserTeam.Budget = 100;
//                db.UserTeams.Update(SelectedUserTeam);
//                db.SaveChanges();
//                return SelectedUserTeam.Budget;
//            }
//        }

//        public void setSelectedNbaTeam(object selectedItem)
//        {
//            SelectedNbaTeam = (Nbateams)selectedItem;
//        }

//        public void setSelectedUserTeam(object selectedItem)
//        {
//            SelectedUserTeam = (UserTeams)selectedItem;
//        }

//        public List<UserTeams> AllUserTeams()
//        {
//            using (var db = new BasketballProjectContext())
//            {
//                return db.UserTeams.ToList();
//            }
//        }

//

//        public void SetSelectedPlayer(object selectedItem)
//        {
//            SelectedPlayers = (Players)selectedItem;
//        }

//        public bool CheckBudget()
//        {
//            if (SelectedPlayers.Price <= SelectedUserTeam.Budget)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public void AddPlayerToUserTeam(object selectedItem)
//        {
//            using(var db = new BasketballProjectContext())
//            {
//                SetSelectedPlayer(selectedItem);

//                var numberOfPlayersInTeam = RetrieveUserTeamsPlayers(SelectedUserTeam).Count(); // Get all players in team

//                //Find the user team in the database
//                var userTeam1 =
//                    from u in db.UserTeams
//                    where u.UserTeamId == SelectedUserTeam.UserTeamId
//                    select u;

//                var userTeam = userTeam1.FirstOrDefault();

//                //Find the players in the user team
//                var searchForPlayers=
//                    from UserTeamPlayers in db.UserTeamPlayers
//                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
//                    select UserTeamPlayers;

//                //Check if the player is in the team
//                var isPlayerAlreadyInTeam = searchForPlayers.Count();

//                //Ensure the player has less than 6 players in their team and then the player is in budget
//                if (isPlayerAlreadyInTeam != 1)
//                {
//                    if(numberOfPlayersInTeam < 6)
//                        if (CheckBudget() == true)
//                        {
//                            //Update budget and then add the player
//                            userTeam.Budget -= SelectedPlayers.Price;
//                            db.UserTeams.Update(userTeam);
//                            db.Add(new UserTeamPlayers { UserTeamId = SelectedUserTeam.UserTeamId, PlayerId = SelectedPlayers.PlayerId });
//                            db.SaveChanges();
//                            setSelectedUserTeam(userTeam);
//                        }
//                        else
//                        {
//                            throw new OutOfBudgetException();
//                        }
//                    else
//                    {
//                        throw new TooManyPlayerException();
//                    }
//                }
//            }
//        }

//        public bool IsPlayerInTeam(object selectedItem)
//        {
//            using (var db = new BasketballProjectContext())
//            {
//                //Set the Selected Player
//                SetSelectedPlayer(selectedItem);

//                //Search to see if player is in the users team
//                var searchForPlayers =
//                    from UserTeamPlayers in db.UserTeamPlayers
//                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
//                    select UserTeamPlayers;

//                var isPlayerAlreadyInTeam = searchForPlayers.Count();

//                //if player is in team return true
//                if (isPlayerAlreadyInTeam == 1)
//                {
//                    return true;
//                }
//                return false;
//            }
//        }

//        public void RemovePlayerFromTeam()
//        {
//            using (var db = new BasketballProjectContext())
//            {
//                //Check if the player is in the team, if they are then find the specfic player
//                if (IsPlayerInTeam(SelectedPlayers))
//                {
//                    var selectedPlayer =
//                         from UserTeamPlayers in db.UserTeamPlayers
//                         where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
//                         select UserTeamPlayers;

//                    //Update the budget and then remove the player
//                    SelectedUserTeam.Budget += SelectedPlayers.Price;
//                    db.UserTeams.Update(SelectedUserTeam);
//                    db.SaveChanges();
//                    db.UserTeamPlayers.RemoveRange(selectedPlayer);
//                    db.SaveChanges();
//                }
//            }
//        }

//        public virtual object MakeNewUserTeam(string text)
//        {
//            using(var db = new BasketballProjectContext())
//            {
//                //Set the User
//                var users =
//                    from u in db.Users
//                    where u.UserId == SelectedUser.UserId
//                    select u;

//                SelectedUser = users.FirstOrDefault();

//                //Add the user team and then update the selectedTeam to be the new team created
//                var userTeam = new UserTeams { UserId = SelectedUser.UserId, Budget = 100, TeamName = text};
//                db.UserTeams.Add(userTeam);
//                db.SaveChanges();
//                db.Entry(userTeam).GetDatabaseValues();
//                object newTeam = db.Entry(userTeam).Entity;
//                setSelectedUserTeam(newTeam);
//                return newTeam;
//            }
//        }

//        public void RemoveUserTeam()
//        {
//            using (var db = new BasketballProjectContext())
//            {
//                //Find The Players in the team
//                var searchForPlayers =
//                    from UserTeamPlayers in db.UserTeamPlayers
//                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId)
//                    select UserTeamPlayers;

//                //Check if the the player is in the userTeam
//                var isPlayersInUserTeam = searchForPlayers.Count();

//                //If they are in the team then remove them
//                if(isPlayersInUserTeam > 0)
//                {
//                    db.UserTeamPlayers.RemoveRange(searchForPlayers);
//                    db.SaveChanges();
//                }

//                //Then find the user team and remove the whole team
//                var userTeam =
//                    from ut in db.UserTeams
//                    where ut.UserTeamId == SelectedUserTeam.UserTeamId
//                    select ut;

//                db.UserTeams.RemoveRange(userTeam);
//                db.SaveChanges();
//            }
//            }
//    }
//}