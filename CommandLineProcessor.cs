/////////////////////////////////////////////////////////////////////////
///  CommandLineProcessor.cs  -  Processes the command line            //
///  ver: 1.0                                                          //
///  Language:    C#                                                   //
///  Platform:    Dell Dimension 8100, Windows 2000 Prof., SP2         //
///  Application: CompositeTextAnalyzer, fall 2013                     //
///  Author:      Siddhartha Kakarla, Syracuse University              //
///               (315) 440-5801, skakarla@syr.edu                     //
///  Description: Reads the command line and stores each arguments in  //
///  respective data strutures. This package will be used by every     //
///  other package to process and get the command line arguments.      //
///  Functions:   Constructor(0 args), processCmdLine(string[]),       //
///               display(string[]), getter methods.                   //
///  Date:        10/07/2013                                           //
///                                                                    //
/////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace CompositeTextAnalyzer
{
  public class CommandLineProcessor
  {
    //instance variables to store the commandline arguments 
    List<string> inputfilepatterns;
    List<string> filepatterns;
    public List<string> textqueries;
    public List<string> metadataqueries;
    public string path;
    public bool matchalltextqueries;
    public bool recursivefilesearch;

    //0-arg constructor
    public CommandLineProcessor()
    {
      inputfilepatterns = new List<string>();
      filepatterns = new List<string>();
      textqueries = new List<string>();
      metadataqueries = new List<string>();
      matchalltextqueries=false;
      recursivefilesearch=false;    
      path=null;
    }
    
    // Reads the comamnand line arguments and places them in thier respective instance variables
    public void processCmdLine(string[] args)
    {
      string temparg;
      filepatterns.Add(".txt");
      filepatterns.Add(".cs");
      filepatterns.Add(".cpp");
      filepatterns.Add(".c");

      foreach (string arg in args)
      {
        foreach (string filepattern in filepatterns)
        {
          if (arg.EndsWith(filepattern))
             inputfilepatterns.Add(filepattern);
        }

        if (arg.StartsWith("/T"))
        {
           textqueries.Add(arg.Substring(2, arg.Length - 2));
        }

        if (arg.StartsWith("/M"))
        {
           temparg= arg.Substring(2, arg.Length - 2);
           string[] metadatatags = temparg.Split(',');
           foreach (string iterator in metadatatags)
             metadataqueries.Add(iterator);
        }

        if (arg.Equals("/A"))
        {
          matchalltextqueries = true;
        }

        if (arg.Equals("/R"))
        {
          recursivefilesearch = true;
        }

        if(arg.StartsWith(".."))
          path=arg;

      }
      
    }//end of processCmdLine(-)

      //Dispalys the command line arguments in an ordered manner
      public void display(string[] args)
      {
        
        Console.Write("\n 1)The file patterns given and supported are:");
      
        foreach (string iterator in inputfilepatterns)
        {
          Console.Write("{0}  ", iterator);

        }

        Console.Write("\n 2)The file path specified is: {0}", path);

        Console.Write("\n 3)The text queries are: ");
      
        foreach (string iterator in textqueries)
        {
          if (textqueries.Count == 1)
          {
            Console.Write("{0}  ", iterator);
          }
          else
            Console.Write("{0}, ", iterator);
          
        }

        Console.Write("\n 4)The metadata queries are: ");
        foreach (string iterator in metadataqueries)
        {
          if (metadataqueries.Count == 1)
          {
             Console.Write("{0}", iterator);
          }
          else
             Console.Write("{0},", iterator);
       }

          Console.Write("\n 5)Should match all text quries (/A): {0}", matchalltextqueries);
          Console.Write("\n 6)Recursive File Search Enabled (/R): {0}", recursivefilesearch);
          
          
      }//end of display(-)


      //getter methods for instance variables
      public string getPath()
      {
        return this.path;
      }

      public List<string> getInpuFilePatterns()
      {
          return this.inputfilepatterns;
      }

      public List<string> getFilePatterns()
      {
        return this.filepatterns;
      }

      public List<string> getMetaDataQueries()
      {
        return this.metadataqueries;
      }

      public List<string> getTextQueries()
      {
        return this.textqueries;
      }

      public bool getMatchAllQueries()
      {
        return this.matchalltextqueries;
      }

      public bool getRecursiveFileSearch()
      {
        return this.recursivefilesearch;
      }

      //A factory method which returns a CommandLineProcessor object
      public static CommandLineProcessor objFactory()
      {
        return new CommandLineProcessor();
      }

     
      //test stub
      #if(TEST_CommandLineProcessor)
        [STAThread]
        static void Main(string[] args)
        {
              Console.Write("\n This is CommandLineProcessor.cs");
              CommandLineProcessor cmdlineproc = new CommandLineProcessor();
              cmdlineproc.processCmdLine(args);
              cmdlineproc.display(args);
        }
  
      #endif

  }//End of CommandLineProcessor class

}//end of CompositeTextAnalyzer namespace
    