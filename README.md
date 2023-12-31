# Shelf - A different way to manage files - V1.0

With shelf, you can manage text files (**split, merge, add or delete text**) and folders(**split**)

## Table Of Contents
- [Shelf - A different way to manage files - V1.0](#shelf---a-different-way-to-manage-files---v10)
	* [Switch](#switch)
    * [Splitter Code Examples](#splitter-code-examples)
    * [Merger Code Example](#merger-code-example)

## Switch
Inside of our parser, we take a array of chars (user input) and match them against a switch.

```csharp
 switch (aChar)
 {
     case '|':
     {
         parserMerger.CreateMergedFile(file1,file2);
         break;
     }
     case '+':
     {
         file1.AppendText();
         break;
     }
     case '-':
     {
         file1.DeleteText();
         break;
     }
     case '\\':
     {
         parserSplitter.CreateFileSplice(file1,"DOWN");
         break;
     }
     case '/':
     {
         parserSplitter.CreateFileSplice(file1,"UP");
         break;
     }
 }
```
In the example above, we have already created a Splitter outside the switch, held in the variable parserSplitter.
### Splitter Code Examples
```csharp
case '/':
{
  parserSplitter.CreateFileSplice(file1, "UP");
  break;
}
```
```csharp
case '\\':
{
  parserSplitter.CreateFileSplice(file1, "DOWN");
  break;
}
```
In the above snippets, we have parameters for a file input, and a direction.
This is used to tell the splitter which half of the text to store and create the new file from.

These both activate this piece of code:
```csharp
 switch (whichWay)
{
    case "UP":
   	{
        properFileText = readFileText[..lengthOfSlice];
        newFileSize = properFileText.Length * sizeof(char);
                    
        tempFile = new File(
            $"split-up-file-{DateTime.Now}",
            @"C:\\Users\\xande\\testing",
             ".txt",
            properFileText,
            DateTime.Now,
             ewFileSize);
        break;
    }
    case "DOWN":
    {
        properFileText = readFileText[lengthOfSlice..];
        newFileSize = properFileText.Length * sizeof(char);
                    
        tempFile = new File(
            $"split-down-file-{DateTime.Now}",
            @"C:\\Users\\xande\\testing",
            ".txt",
            properFileText,
            DateTime.Now,
            newFileSize);
        break;
    }
}
```

### Merger Code Example
```csharp
case '|':
{
    parserMerger.CreateMergedFile(file1,file2);
    break;
}
```
This snippet activates this:
```csharp
 public void CreateMergedFile(File? file1, File? file2)
{
    string readFileText, readFileText2;
    long newFileSize;
    File? tempFile = null;
            
    readFileText = DocumentDataRead(file1);
    readFileText2 = DocumentDataRead(file2);
    var fullText = readFileText + readFileText2;
            
    newFileSize = fullText.Length * sizeof(char);

    tempFile = new File($"split-up-file-{DateTime.Now}",
        @"C:\Users\$USER\shelf-output",
        ".txt",
        fullText,
        DateTime.Now,
        newFileSize);

    if (tempFile?.Path == null) return;
    SysIO.File.Create(tempFile.Path);
    InsertSingleFileData(DbConn,tempFile);
```