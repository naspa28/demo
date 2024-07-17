using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public List<GameSession> vm { get; set; }
    public List<GameSession> sp { get; set; }
    public List<GameSession> fg { get; set; }
    public List<GameSession> pc { get; set; }
    public List<GameSession> sr { get; set; }

    public GameData()
    {
        vm = new List<GameSession>();
        sp = new List<GameSession>();
        fg = new List<GameSession>();
        pc = new List<GameSession>();
        sr = new List<GameSession>();
    }
}

[Serializable]
public class GameSession
{
    public string date;
    public int lvl;
    public int star;
    public int try_count;
    public float corr;
    public int time;
    public int conc;

    public GameSession(string date, int lvl, int star, int try_count, float corr, 
        int time, int conc)
    {
        this.date = date;
        this.lvl = lvl;
        this.star = star;
        this.try_count = try_count;
        this.corr = corr;
        this.ans_rate = ans_rate;
        this.time = time;
        this.conc = conc;
    }
}
