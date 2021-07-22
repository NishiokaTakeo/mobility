import { ElementSchemaRegistry } from '@angular/compiler';
import { Injectable } from '@angular/core';
//import { exception } from 'console';

@Injectable({
  providedIn: 'root'
})

export class SearchHistoryService {
  key; string = 'searchHistory';
  storage: string[] =[];

  constructor() 
  {
    this.init();
  }
  
  public get()
  {
    return this.storage;
  }

  public set(item:string)
  {
    if ( item === null || item ==='' )
    {
      throw new Error("Argument was not passed.");
    }
    else
    {
      this.storage.push(item);
      localStorage.setItem(this.key,JSON.stringify(this.storage));
    }
  }

  init()
  {
    let item:string = localStorage.getItem(this.key);
    
    if ( item == null || item === "" )
    {
      localStorage.setItem(this.key,JSON.stringify(this.storage));
    }
    else
    {
      this.storage = JSON.parse(item);
    }

  }  


}
