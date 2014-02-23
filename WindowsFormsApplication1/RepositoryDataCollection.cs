using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;


class RepositoryData
{
    public string url = "";
    public int revision = 0;
    private enum State
    {
        NO_CHECK, CHECKED, IMPORT
    }
    private State state = State.IMPORT; 
    public RepositoryData(string _url, int _revision)
    {
        this.url = _url;
        this.revision = _revision;
    }
    public override string ToString()
    {
        return string.Format("url : {0} revision : {1}", url, revision);
    }
    public bool IsUpdated()
    {
        return this.state == State.NO_CHECK;
    }
    public bool IsChecked()
    {
        return this.state == State.CHECKED;
    }
    public void Update()
    {
        this.state = State.NO_CHECK;
    }
    public void Check()
    {
        this.state = State.CHECKED;
    }
}

class RepositoryDataCollection : KeyedCollection<string, RepositoryData>
{
    protected override string GetKeyForItem(RepositoryData item)
    {
        return item.url;
    }
}