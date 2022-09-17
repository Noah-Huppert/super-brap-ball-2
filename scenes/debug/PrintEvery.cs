using Godot;

public class PrintEvery
{
    private int lastPrint;
    private int every;
    private bool noPrefix;

    // Initializes a PrintEvery.
    // every - The Print() method must be called this many times before a value is actually printed.
    // noPrefix - If false all print statements will have some indication that they aren't continuously printed, if false just the raw msg is printed.
    public PrintEvery(int every, bool noPrefix = false)
    {
        this.lastPrint = 0;
        this.every = every;
        this.noPrefix = noPrefix;
    }

    public void Print(string msg)
    {
        if (this.lastPrint >= this.every)
        {
            GD.Print((this.noPrefix ? "" : "[every " + this.every + "]: ") + msg);
            this.lastPrint = 0;
        } else
        {
            this.lastPrint += 1;
        }
    }
}
