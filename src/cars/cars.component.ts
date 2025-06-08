import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import * as XLSX from 'xlsx';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-cars',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.css']
})
export class CarsComponent implements OnInit {
  cars: any[] = [];
  displayedColumns: string[] = ['id', 'carBrand', 'carColour', 'carPrice', 'modelDate', 'inStock'];
  public router: Router;

  constructor(private http: HttpClient, router: Router, private cdr: ChangeDetectorRef) {
    this.router = router;
  }

  ngOnInit() {
    this.fetchCars();
  }

  async fetchCars() {
    try {
      const data = await firstValueFrom(this.http.get<any[]>('http://localhost:5073/api/cars'));
      console.log('Fetched data:', data);
      this.cars = data || [];
      this.cdr.detectChanges();
    } catch (error) {
      console.error('Error fetching cars:', error);
    }
  }

  onExport() {
    const worksheet = XLSX.utils.json_to_sheet(this.cars);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Cars');
    XLSX.writeFile(workbook, 'Cars.xlsx');
  }

  // Sorting logic
  sortData(column: string) {
    this.cars.sort((a, b) => {
      const aValue = a[column];
      const bValue = b[column];
      if (typeof aValue === 'string') return aValue.localeCompare(bValue);
      if (typeof aValue === 'number') return aValue - bValue;
      if (aValue instanceof Date) return aValue.getTime() - bValue.getTime();
      return 0;
    });
    this.cdr.detectChanges();
  }

  // Filtering logic (simple text filter)
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value?.trim().toLowerCase() || '';
    this.cars = this.cars.filter(car =>
      Object.values(car).some(value =>
        value?.toString().toLowerCase().includes(filterValue)
      )
    );
    this.cdr.detectChanges();
  }
}