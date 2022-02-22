import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [
    NavbarComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  exports:[
    NavbarComponent
  ]
})
export class CoreModule { }
