import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Chef {
  id: number;
  Name: string;
  Cuisine: string;
}

@Injectable({ providedIn: 'root' })
export class ChefService {
  constructor(private http: HttpClient) { }

  getChefs() {
    return this.http.get<Chef[]>('/api/chefs');
  }
}
