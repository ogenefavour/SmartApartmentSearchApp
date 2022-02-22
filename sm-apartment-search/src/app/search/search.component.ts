import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { of } from 'rxjs';
import { debounceTime, switchMap, tap } from 'rxjs/operators';
import { ServiceService } from '../core/services/service.service';
import { MarketList, SearchResultContents } from '../shared/interfaces';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  markets: MarketList[] = [];

  loading:boolean = true;

  searchForm: FormGroup = new FormGroup({});

  searchContent: any[] = [];

  constructor(private formBuilder: FormBuilder,
              private searchService: ServiceService) { }


  ngOnInit(): void {

    this.buildFileForm();

    this.getStates();

    this.getSearchResult();

  }

  buildFileForm() {
    this.searchForm = this.formBuilder.group({
      searchInput: ['', [Validators.required]],
      marketSelector: ['', [Validators.required]]
    });
  }

  getStates(){
    this.searchService.getMarket()
      .subscribe(s =>{
        this.markets = s
      });
  }


  getSearchResult(){
    let multiselectValue = this.searchForm.get('marketSelector')?.value;
    console.log(multiselectValue.toString().replace(/%20/i, ' '));
    this.searchForm.get('searchInput')?.valueChanges
      .pipe(
        tap(a => console.log(a)),
        debounceTime(400),
        switchMap(searchTerm => this.searchService.getAppartmentSearch(searchTerm, multiselectValue.toString().replace(/%20/i, ' ')))
      ).subscribe(sc =>{
        //console.log('data is' + JSON.stringify(sc))
        this.searchContent = sc;

      },
      err => console.log(err),
      () => this.loading = false)
  }

  onStateChange(){
    console.log('selected input changed');
    let searchQuery = this.searchForm.get('searchInput')?.value;

    let multiselectValue = this.searchForm.get('marketSelector')?.value;

    // let markets = new Array()
    // markets = multiselectValue;
    //console.log(markets);

    if (searchQuery !== null) {
      of(searchQuery)
      .pipe(switchMap(searchTerm => this.searchService.getAppartmentSearch(searchTerm, multiselectValue.toString().replace(/%20/i, ' ')))).subscribe(
        sc => this.searchContent = sc,
        err => console.log(err),
        () => this.loading = false
      )
    }
    else{
      console.log('form not valid yet');
    }
  }

}
