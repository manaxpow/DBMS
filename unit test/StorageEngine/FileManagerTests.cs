using System;
using Xunit;

public class FileManagerTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void CreateFile_WhenPathIsValid_ShouldCreateFile()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void OpenFile_WhenFileExists_ShouldReturnHandle()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DeleteFile_WhenFileIsInUse_ShouldThrow()
    {
        throw new NotImplementedException();
    }


    [Trait("Category", "Important")]
    [Fact]
    public void CreateFile_WhenFileAlreadyExists_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CreateFile_WhenPathIsInvalid_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void CreateFile_WhenPhysicalCreationFails_ShouldNotRegisterFile()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void OpenFile_WhenFileDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void OpenFile_WhenAccessModeConflicts_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void ReadPage_WhenFileIsOpen_ShouldReturnData()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ReadPage_WhenFileIsClosed_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void WritePage_WhenFileIsReadWrite_ShouldWriteData()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void WritePage_WhenFileIsReadOnly_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void CloseFile_WhenFileIsOpen_ShouldCloseHandle()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CloseFile_WhenFileIsAlreadyClosed_ShouldRemainClosed()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DeleteFile_WhenFileIsNotOpen_ShouldDeleteFile()
    {
        throw new NotImplementedException();
    }
}

