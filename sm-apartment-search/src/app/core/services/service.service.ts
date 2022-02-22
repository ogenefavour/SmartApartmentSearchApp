import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MarketList, SearchResult, SearchResultContents } from 'src/app/shared/interfaces';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { map, catchError, tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  constructor(private http: HttpClient) { }


  getMarket(): Observable<MarketList[]> {
    return this.http.get<MarketList[]>(`${environment.ExternalService.FileService.baseUrl}api/Search/GetMarketList`)
        .pipe(
            tap(c => console.log(c)),
            catchError(this.handleError)
        );
  }


  getAppartmentSearch(searchquery:string, market:string): Observable<any[]> {
    return this.http.get<any[]>(`${environment.ExternalService.FileService.baseUrl}api/Search/SearchSmartApartment?searchPhrase=${searchquery}&market=${market}`)
        .pipe(
            tap(n => console.log(n)),
            catchError(this.handleError)
        );
  }


  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error);
    if (error.error instanceof Error) {
        const errMessage = error.error.message;
        return Observable.throw(errMessage);
    }
    return Observable.throw(error || 'server error');
 }
}
