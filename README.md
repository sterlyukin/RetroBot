# RetroBot

[![build](https://github.com/sterlyukin/RetroBot/actions/workflows/build_validation.yml/badge.svg)](https://github.com/sterlyukin/RetroBot/actions/workflows/build_validation.yml)
[![test](https://github.com/sterlyukin/RetroBot/actions/workflows/test_validation.yml/badge.svg)](https://github.com/sterlyukin/RetroBot/actions/workflows/test_validation.yml)

[Join RetroBot](https://t.me/ZhiSteRetroBot) (@ZhiSteRetroBot)

This is telegram bot that helps conduct retro process in teams.

# How it works

RetroBot is based on simple principles of retro in teams: each team member should answer on 3 questions:
- what we should stop to do?
- what we should start to do?
- what we should continue to do?

RetroBot interaction algorithm:

1) When you join RetroBot you should press `Start` button;
2) If you want to create new you should press `Create team` button;
3) Then you should to enter team name;
4) After that you should enter teamlead email, on which RetroBot will send reports;
5) At the end of the process RetroBot will send you team id that allows your team members to join your team. They can make it if they will press `Join team` button on the second step;
6) Once a week RetroBot will send you 3 questions in sequence about the team, which were given above. Each team member must answer on them and RetroBot will send anonymous generalized report on the command to it's teamlead's email.
