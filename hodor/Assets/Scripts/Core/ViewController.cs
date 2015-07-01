using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Must;

/// ViewController is a base class for all view controllers.
///
/// It is responsible for initializing and updating any properties
/// exposed by a view presenter. Any and all UI logic should be contained
/// within the ViewController subclasses.
public class ViewController<ViewPresenterT> : SimpleViewController
                                              where ViewPresenterT : ViewPresenter
{
    [HideInInspector]
    public ViewPresenterT ViewPresenter;

    void Awake()
    {
        ViewPresenter = gameObject.GetComponent<ViewPresenterT>();

        ViewPresenter.MustNotBeNull();
    }
}

/// SimpleViewController is a specialized view controller that does not require a view presenter
public class SimpleViewController : MonoBehaviour
{

}
