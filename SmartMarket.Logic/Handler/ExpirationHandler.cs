using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Handler;

public abstract class ExpirationHandler : IExpirationHandler
{
    private IExpirationHandler _nextHandler;

    public void SetNextHandler(IExpirationHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public bool Handle(int currentAge, StockItem stockItem)
    {
        if (CanHandle(currentAge, stockItem))
        {
            
            return true;
        }

        if (_nextHandler != null)
        {
            return _nextHandler.Handle(currentAge, stockItem);
        }

        return false;
    }

    protected abstract bool CanHandle(int currentAge, StockItem stockItem);

    
}