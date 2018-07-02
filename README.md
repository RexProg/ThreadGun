# ThreadGun
ThreadGun is a form of multithreading created for developers keen on building fast and stable application.

# Features
- Support async method
- Will be notify when exception occurred
- Will be notify when process completed
- Postpone the process
- Join to process
- Optimum perfomance
- Easy [Installation](https://github.com/RexProg/ThreadGun#Installation) using [NuGet](http://nuget.org/packages/ThreadGun)

# ThreadPool vs ThreadGun
- ThreadGun is more accurate than ThreadPool
- ThreadGun is faster than ThreadPool
![Form](Form.png)
![dotTrace](dotTrace.png)

# Examples
+ See the [Example project](https://github.com/RexProg/ThreadGun/tree/master/TestThreadingMethod).
+ See the [SiteBruteForcer](https://github.com/RexProg/SiteBruteForcer).
+ See the [ProxyGather](https://github.com/RexProg/ThreadGun/tree/master/ProxyGather).

# Getting Started
First, add ThreadGun to your project using [NuGet](https://github.com/RexProg/ThreadGun#Installation).
look at Simple [Example](https://github.com/RexProg/ThreadGun/blob/master/TestThreadingMethod/TestForm.cs)
```csharp
private void btnThreadGun_Click(object sender, EventArgs e)
{
    lstThreadGunResult.Items.Clear();
    var tg = new ThreadGun<int>(ActionThreadGun, Enumerable.Range(1, 50000), 20);
    tg.Completed += tg_Completed;
    tg.ExceptionOccurred += tg_ExceptionOccurred;
	tg.FillingMagazine().Start();
}
private void tg_ExceptionOccurred(IEnumerable<int> inputs, Exception exception)
{
    MessageBox.Show(exception.Message);
}
private void tg_Completed(object inputs)
{
    MessageBox.Show(@"ThreadGun Process Completed!");
}
public void ActionThreadGun(int i)
{
    Invoke(new MethodInvoker(delegate ()
    {
		lstThreadGunResult.Items.Add($@"> {i} <");
        lblInfoThreadGun.Text = $@"Item Count : {lstThreadGunResult.Items.Count}";
    }));
	Application.DoEvents();
	if (i == 250)
		throw new Exception("ExceptionOccurred Test!");
}
```
If you have a method with more than one parameter, you can put the parameters in another class and change method's input type to it.

For Example :

```csharp
public void ActionThreadGun(int i,int j)
{
    Invoke(new MethodInvoker(delegate ()
    {
		lstThreadGunResult.Items.Add($@"> {i} , {j} <");
    }));
    Application.DoEvents();
    if (i == 250)
      throw new Exception("ExceptionOccurred Test!");
}
```
Change it to :
```csharp
public void ActionThreadGun(Point p)
{
    Invoke(new MethodInvoker(delegate ()
    {
		lstThreadGunResult.Items.Add($@"> {p.i} , {p.j} <");
    }));
    Application.DoEvents();
    if (p.i == 250)
      throw new Exception("ExceptionOccurred Test!");
}
public class Point
{
  public int i { get; set; }
  public int j { get; set; }
}
```
```csharp
new ThreadGun<Point>((Action<Point>)ActionThreadGun, new List<Point>() { new Point() { i = 20, j = 40 } }, 20);
```

# Usage
```csharp
new ThreadGun<T>((Action<T>)ActionByParameter, IEnumerable<T>, ThreadCount)
```
or
```csharp
new ThreadGun<T>(Func, IEnumerable<T>, ThreadCount)
```
or
```csharp
new ThreadGun<T>(Action, MagazineCount, ThreadCount)
```
- T is type of inputs
- ActionByParameter is the method that you want to invoke and type of that method's parameter is T
- Action(WithoutParameter) is the method that you want to invoke
- Func is the async method that you want to invoke and type of that method's parameter is T
- IEnumerable<T> is your input which type of that is T
- MagazineCount is count of method executed
- ThreadCount is count of thread executed action at same time

# Installation

Install as [NuGet package](https://www.nuget.org/packages/ThreadGun):

Package manager:

```powershell
Install-Package ThreadGun
```
