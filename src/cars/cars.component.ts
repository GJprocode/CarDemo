import { Component } from '@angular/core';
import { GridComponent } from '../grid/grid.component';

@Component({
  selector: 'app-cars',
  standalone: true,
  imports: [GridComponent],
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.css']
})
export class CarsComponent {}
