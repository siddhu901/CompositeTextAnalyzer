/////////////////////////////////////////////////////////////////////////
///  Executive.cs  -  The main package which is the entry point of the //
///                   application and which drives it through various  //
///                   phases.                                          //
///  ver: 1.0                                                          //
///  Language:    C#                                                   //
///  Platform:    Dell Dimension 8100, Windows 2000 Prof., SP2         //
///  Application: CompositeTextAnalyzer, fall 2013                     //
///  Author:      Siddhartha Kakarla, Syracuse University              //
///               (315) 440-5801, skakarla@syr.edu
///  Source:      Jim Fawcett, CST 2-187, Syracuse University          //
///               (315) 443-3948, jfawcett@twcny.rr.com                //
///  Description: Demonstrates how each requirement of project is met  //
///               in the application using /N switch in the            //
///               commandline.                                         //
///  Functions:   Main(string[] ),displayTitle(string,char),           //
///               displayCommandLineArgs(string[])                     //
///  Date:        10/07/2013                                           //
/////////////////////////////////////////////////////////////////////////
using System;

namespace CompositeTextAnalyzer
{
  class Executive
  {
    //displays a specified title decorated with a specified character
    static void displayTitle(string title, char underlineWith = '=')
    {
      Console.Write("\n  {0}", title);
      Console.Write("\n {0}", new string(underlineWith, title.Length + 2));
    }
    
    //displays the commandline argument
    static void displayCommandLineArgs(string[] args)
    {
      Console.Write("\n  ");
      foreach (string arg in args)
        Console.Write(" {0}", arg);
      Console.Write("\n");
    }
    
    //Entry point of the application
    static void Main(string[] args)
    {
      displayTitle("Demonstrating how I met the Requirements");
      Console.Write("\n  command line arguments are: ");
      displayCommandLineArgs(args);
      bool found = false;
      foreach (string arg in args)
      {
        if(arg.StartsWith("/N"))
        {
          found = true;
          int reqNum = Int32.Parse(arg.Substring(2));
          Console.Write("\n  \t\t\tDemonstrating requirement {0}", reqNum);
          switch(reqNum)
          {
            case 2: //Uses the CommandLineProcessor package to process and print the command line arguments 
              {
                CommandLineProcessor cmdlineproc = new CommandLineProcessor();
                cmdlineproc.processCmdLine(args);
                cmdlineproc.display(args);
                break;
              }
            case 3: //Uses the TextQueries package to search for text queries in the file set
              {
                TextQueries textqueries = new TextQueries();
                textqueries.processQuery(args);
                break;
              }
            case 4: //Uses the MatdataQueries package to search for metadata queries in the xml file set
              {
                MetadataQueries metadataqueries = new MetadataQueries();
                metadataqueries.processQuery(args); 
               break;
              }
            case 5: 
              {
                Console.Write("\n This requirement is already satisfied while seraching for text and metadata quries. Check that !!");
                break;
              }
            case 6:
              {
                Console.Write("\n Run the matadata tool as a separate project !!"); 
                break;
              }
            default:
              {
                Console.Write("\n    no such requirement");
                break;
              }
          }
        }
      }
      if (found == false)
        Console.Write("\n   this content is based on the command line arguments");
      Console.Write("\n\n");
    }
  }// End of Executive class

}//end of CompositeTextAnalyzer namespace
