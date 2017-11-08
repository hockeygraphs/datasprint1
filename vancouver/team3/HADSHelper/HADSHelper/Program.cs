using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualBasic.FileIO;

namespace HADSHelper
{
    class Coordinate
    {
        public Coordinate(string rawData)
        {
            rawData = rawData.Replace("(", "");
            rawData = rawData.Replace(")", "");

            string[] splits = rawData.Split(',');
            if (splits.Count() == 2)
            {
                X = int.Parse(splits[0]);
                Y = int.Parse(splits[1]);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }

        public int X = 0;
        public int Y = 0;
    }

    class GameEvent
    {
        public GameEvent(uint eventId, uint gameId, uint teamId, uint period, 
            string eventType, string subType, uint mainPlayer, uint secondPlayer, uint thirdPlayer,
            string mainLocation, string secondaryLocation, uint homeTeam, uint awayTeam,
            string venue, string leagueName, string gameDate, string homeStartsLeft, uint seconds,
            uint eventIndex, uint homePlayerOne, uint homePlayerTwo, uint homePlayerThree, uint homePlayerFour,
            uint homePlayerFive, uint homePlayerSix, uint awayPlayerOne, uint awayPlayerTwo, uint awayPlayerThree, uint awayPlayerFour,
            uint awayPlayerFive, uint awayPlayerSix, uint homeGoalie, uint awayGoalie, string eng, string awayState, string homeState,
            string gameState, string otherGameState)
        {
            EventId = eventId;
            GameId = gameId;
            TeamId = teamId;
            Period = period;
            EventType = eventType;
            SubType = subType;
            MainPlayerId = mainPlayer;
            SecondPlayerId = secondPlayer;
            ThirdPlayerId = thirdPlayer;
            MainLocation = new Coordinate(mainLocation);
            SecondaryLocation = new Coordinate(secondaryLocation);
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;

            Venue = venue;
            LeagueName = leagueName;
            GameDate = gameDate;
            HomeStartsLeft = homeStartsLeft;
            Seconds = seconds;
            EventIndex = eventIndex;

            HomePlayers = new uint[6];

            HomePlayers[0] = homePlayerOne;
            HomePlayers[1] = homePlayerTwo;
            HomePlayers[2] = homePlayerThree;
            HomePlayers[3] = homePlayerFour;
            HomePlayers[4] = homePlayerFive;
            HomePlayers[5] = homePlayerSix;

            AwayPlayers = new uint[6];

            AwayPlayers[0] = awayPlayerOne;
            AwayPlayers[1] = awayPlayerTwo;
            AwayPlayers[2] = awayPlayerThree;
            AwayPlayers[3] = awayPlayerFour;
            AwayPlayers[4] = awayPlayerFive;
            AwayPlayers[5] = awayPlayerSix;

            HomeGoalie = homeGoalie;
            AwayGoalie = awayGoalie;
            Eng = eng;

            AwayState = awayState;
            HomeState = homeState;

            GameState = gameState;
            OtherGameState = otherGameState;
        }

        public uint EventId;
        public uint GameId;
        public uint TeamId;
        public uint Period;
        public string EventType;
        public string SubType;
        public uint MainPlayerId;
        public uint SecondPlayerId;
        public uint ThirdPlayerId;

        public Coordinate MainLocation;
        public Coordinate SecondaryLocation;

        public uint HomeTeam;
        public uint AwayTeam;

        public string Venue;
        public string LeagueName;
        public string GameDate;
        public string HomeStartsLeft;
        public uint Seconds;
        public uint EventIndex;

        public uint[] HomePlayers;
        public uint[] AwayPlayers;

        public uint HomeGoalie;
        public uint AwayGoalie;

        public string Eng;
        public string AwayState;
        public string HomeState;
        public string GameState;
        public string OtherGameState;
    }

    struct HitChainData
    {
        public HitChainData(uint timeToPossessionChange, int eventCount, string eventType, Coordinate hitLocation, Coordinate changePossessionLocation)
        {
            TimeToPossessionChange = timeToPossessionChange;
            EventCountToPossessionChange = eventCount;
            EventTypeOfPossessionChange = eventType;
            HitLocation = hitLocation;
            ChangePossessionLocation = changePossessionLocation;
        }

        public uint TimeToPossessionChange;
        public int EventCountToPossessionChange;
        public string EventTypeOfPossessionChange;
        public Coordinate HitLocation;
        public Coordinate ChangePossessionLocation;

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}, {4}", TimeToPossessionChange, EventCountToPossessionChange, EventTypeOfPossessionChange, HitLocation.ToString(), ChangePossessionLocation.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (TextFieldParser csvParser = new TextFieldParser(@"C:\HADS\final_data.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                List<GameEvent> allEvents = new List<GameEvent>();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    uint fieldIdx = 1;

                    uint eventId = SafeGetUint(fields[fieldIdx++]);
                    uint gameId = SafeGetUint(fields[fieldIdx++]);
                    uint teamId = SafeGetUint(fields[fieldIdx++]);
                    uint period = SafeGetUint(fields[fieldIdx++]);
                    string eventType = fields[fieldIdx++];
                    string subType = fields[fieldIdx++];
                    uint mainPlayerId = SafeGetUint(fields[fieldIdx++]);
                    uint secondPlayerId = SafeGetUint(fields[fieldIdx++]);
                    uint thirdPlayerId = SafeGetUint(fields[fieldIdx++]);
                    string mainLocation = fields[fieldIdx++];
                    string secondaryLocation = fields[fieldIdx++];
                    uint homeTeam = SafeGetUint(fields[fieldIdx++]);
                    uint awayTeam = SafeGetUint(fields[fieldIdx++]);

                    string venue = fields[fieldIdx++];
                    string leagueName = fields[fieldIdx++];
                    string gameDate = fields[fieldIdx++];
                    string homeStartsLeft = fields[fieldIdx++];

                    uint seconds = SafeGetUint(fields[fieldIdx++]);
                    uint eventIndex = SafeGetUint(fields[fieldIdx++]);

                    uint homePlayerOne = SafeGetUint(fields[fieldIdx++]);
                    uint homePlayerTwo = SafeGetUint(fields[fieldIdx++]);
                    uint homePlayerThree = SafeGetUint(fields[fieldIdx++]);
                    uint homePlayerFour = SafeGetUint(fields[fieldIdx++]);
                    uint homePlayerFive = SafeGetUint(fields[fieldIdx++]);
                    uint homePlayerSix = SafeGetUint(fields[fieldIdx++]);

                    uint awayPlayerOne = SafeGetUint(fields[fieldIdx++]);
                    uint awayPlayerTwo = SafeGetUint(fields[fieldIdx++]);
                    uint awayPlayerThree = SafeGetUint(fields[fieldIdx++]);
                    uint awayPlayerFour = SafeGetUint(fields[fieldIdx++]);
                    uint awayPlayerFive = SafeGetUint(fields[fieldIdx++]);
                    uint awayPlayerSix = SafeGetUint(fields[fieldIdx++]);

                    uint homeGoalie = SafeGetUint(fields[fieldIdx++]);
                    uint awayGoalie = SafeGetUint(fields[fieldIdx++]);

                    string eng = fields[fieldIdx++];
                    string awayState = fields[fieldIdx++];
                    string homeState = fields[fieldIdx++];
                    string gameState = fields[fieldIdx++];
                    string otherGameState = fields[fieldIdx++];

                    GameEvent gameEvent = new GameEvent (eventId, gameId, teamId, period, eventType, subType,
                        mainPlayerId, secondPlayerId, thirdPlayerId, mainLocation, secondaryLocation, homeTeam, awayTeam,
                        venue, leagueName, gameDate, homeStartsLeft, seconds, eventIndex, homePlayerOne, homePlayerTwo,
                        homePlayerThree, homePlayerFour, homePlayerFive, homePlayerSix, awayPlayerOne, awayPlayerTwo, awayPlayerThree,
                        awayPlayerFour, awayPlayerFive, awayPlayerSix, homeGoalie, awayGoalie, eng, awayState, homeState, gameState, otherGameState);

                    allEvents.Add(gameEvent);
                }

                List<List<GameEvent>> eventChain = new List<List<GameEvent>>();

                var eventsEnumerator = allEvents.GetEnumerator();


                for (int eventIndex = 0; eventIndex < allEvents.Count; ++eventIndex)
                {
                    GameEvent thisEvent = allEvents[eventIndex];

                    bool keepGoing = true;
                    bool addChain = true;
                    List<GameEvent> possessionChain = new List<GameEvent>();

                    possessionChain.Add(thisEvent);

                    for (eventIndex++; eventIndex < allEvents.Count && keepGoing == true; ++eventIndex)
                    {
                        GameEvent eventInPossessionChain = allEvents[eventIndex];

                        bool addEvent = true;

                        if (eventInPossessionChain.GameState != "5v5")
                        {
                            keepGoing = false;
                            addChain = false;
                        }

                        if (eventInPossessionChain.TeamId != thisEvent.TeamId || eventInPossessionChain.EventType == "goal" || eventInPossessionChain.EventType == "penalty")
                        {
                            keepGoing = false;
                            addEvent = false;
                        }

                        if (eventInPossessionChain.EventType == "faceoff")
                        {
                            keepGoing = false;
                        }

                        if (addEvent)
                        {
                            possessionChain.Add(eventInPossessionChain);
                        }

                        if (!keepGoing)
                        {
                            eventIndex--;
                        }

                    }

                    if (possessionChain.All(x => x.EventType == "penalty"))
                    {
                        addChain = false;
                    }

                    if (addChain)
                    {
                        eventChain.Add(possessionChain);
                    }
                }

                List<List<GameEvent>> eventChainsWithNoHitAbsorbed = new List<List<GameEvent>>();
                List<List<GameEvent>> eventChainsWithHitAbsorbed = new List<List<GameEvent>>();

                foreach (var possessionChain in eventChain)
                {
                    if (possessionChain.Any(x => x.EventType == "hit"))
                        eventChainsWithHitAbsorbed.Add(possessionChain);
                    else
                        eventChainsWithNoHitAbsorbed.Add(possessionChain);
                }

                InterestingData noHitsData = GetInterestingDataFromChain(eventChainsWithNoHitAbsorbed);

                double averagePossessionTimeNoHit = (double)noHitsData.TotalPossessionTime / (double)eventChainsWithNoHitAbsorbed.Count;
                double averageShotsPerHourNoHit = (double)noHitsData.TotalShots / ((double)noHitsData.TotalPossessionTime / (double)3600);
                double averageGoalsPerHourNoHit = (double)noHitsData.TotalGoals / ((double)noHitsData.TotalPossessionTime / (double)3600);
                double averagePassesPerHourNoHit = (double)noHitsData.TotalPasses / ((double)noHitsData.TotalPossessionTime / (double)3600);

                Console.WriteLine("Don't Absorb A Hit:");
                Console.WriteLine("Average Possession Length: {0}", averagePossessionTimeNoHit);
                Console.WriteLine("Average Shots Per Hour: {0}", averageShotsPerHourNoHit);
                Console.WriteLine("Average Goals Per Hour: {0}", averageGoalsPerHourNoHit);
                Console.WriteLine("Average Passes Per Hour: {0}", averagePassesPerHourNoHit);

                InterestingData withHitsData = GetInterestingDataFromChain(eventChainsWithHitAbsorbed);

                double averagePossessionTimeAbsorbedHit = (double)withHitsData.TotalPossessionTime / (double)eventChainsWithHitAbsorbed.Count;
                double averageShotsPerHourAbsorbedHit = (double)withHitsData.TotalShots / ((double)withHitsData.TotalPossessionTime / (double)3600);
                double averageGoalsPerHourAbsorbedHit = (double)withHitsData.TotalGoals / ((double)withHitsData.TotalPossessionTime / (double)3600);
                double averagePassesPerHourAbsorbedHit = (double)withHitsData.TotalPasses / ((double)withHitsData.TotalPossessionTime / (double)3600);

                Console.WriteLine("Absorb A Hit:");
                Console.WriteLine("Average Possession Length: {0}", averagePossessionTimeAbsorbedHit);
                Console.WriteLine("Average Shots Per Hour: {0}", averageShotsPerHourAbsorbedHit);
                Console.WriteLine("Average Goals Per Hour: {0}", averageGoalsPerHourAbsorbedHit);
                Console.WriteLine("Average Passes Per Hour: {0}", averagePassesPerHourAbsorbedHit);

                Console.In.ReadLine();
                
            }
        }

        public class InterestingData
        {
            public InterestingData(uint totalShots, uint totalGoals, uint totalPossesionTime, uint totalPasses)
            {
                TotalShots = totalShots;
                TotalGoals = totalGoals;
                TotalPossessionTime = totalPossesionTime;
                TotalPasses = totalPasses;
            }

            public uint TotalShots;
            public uint TotalGoals;
            public uint TotalPossessionTime;
            public uint TotalPasses;
        }

        public static InterestingData GetInterestingDataFromChain(List<List<GameEvent>> possessionChains)
        {
            uint totalPossessionTime = 0;

            uint totalShots = 0;
            uint totalGoals = 0;
            uint totalPasses = 0;

            foreach (var possessionChain in possessionChains)
            {
                GameEvent firstEvent = possessionChain.First();
                GameEvent lastEvent = possessionChain.Last();

                uint timeOfPossession = lastEvent.Seconds - firstEvent.Seconds;

                totalShots += (uint)possessionChain.Count(x => x.EventType == "shot" || x.EventType == "missed_shot" || x.EventType == "blocked_shot" || x.EventType == "goal");
                totalGoals += (uint)possessionChain.Count(x => x.EventType == "goal");
                totalPasses += (uint)possessionChain.Count(x => x.EventType == "pass");

                totalPossessionTime += timeOfPossession;
            }

            return new InterestingData(totalShots, totalGoals, totalPossessionTime, totalPasses);
        }

        public static uint SafeGetUint(string field)
        {
            if (String.IsNullOrEmpty(field))
                return ~0u;

            return (uint)float.Parse(field);
        }
    }

    
}
