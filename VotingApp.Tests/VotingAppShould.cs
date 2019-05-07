using System;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Lib;
using Xunit;

namespace VotingApp.Tests
{
    public class VotingAppShould
    {
        [Fact]
        public void Start_Test()
        {
            var voting = new Voting();
            voting.Start("C#", "Kotlin");
            Assert.Equal(
                new Dictionary<string, int>() {
                    {"C#", 0},
                    {"Kotlin", 0}
                },
                voting.Votes
            );
        }

        [Fact]
        public void Vote_Test()
        {
            var voting = new Voting();
            voting.Start("C#", "Kotlin");
            voting.Vote("C#", 1);
            Assert.Equal(new Dictionary<string, int>() {
                    {"C#", 1},
                    {"Kotlin", 0}
                },
                voting.Votes);
        }

        [Fact]
        public void Finish_Test()
        {
            var voting = new Voting();
            voting.Start("C#", "Kotlin");
            voting.Vote("C#", 1);
            voting.Finish();

            Assert.Equal("C#", voting.Winner);
        }
    }


}
