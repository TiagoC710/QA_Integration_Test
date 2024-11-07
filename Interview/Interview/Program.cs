using Interview;
using System;

public class Program{
    public static void Main(){
        string sourceDirectory = "MainFolder";
        string targetDirectory = "Target Folder";

        MaintanceFile maintanceFile = new MaintanceFile(sourceDirectory, targetDirectory);
        maintanceFile.ActionOnFile(MaintanceFile.Action.Syncronize);
        maintanceFile.ActionOnFile(MaintanceFile.Action.Delete , "delete.txt");
        maintanceFile.ActionOnFile(MaintanceFile.Action.Syncronize);
        maintanceFile.ActionOnFile(MaintanceFile.Action.Create , "create.txt", "Test for Create");
        maintanceFile.ActionOnFile(MaintanceFile.Action.Copy , "copy.txt" );
        maintanceFile.ActionOnFile(MaintanceFile.Action.Read , "READ.txt" );
    }
}
