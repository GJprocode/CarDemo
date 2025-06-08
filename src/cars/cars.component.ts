import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import * as XLSX from 'xlsx';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-cars',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatSortModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatButtonModule
  ],
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.css']
})
export class CarsComponent implements OnInit {
  dataSource = new MatTableDataSource<any>([]);
  displayedColumns: string[] = ['id', 'carBrand', 'carColour', 'carPrice', 'modelDate', 'inStock'];
  sortDirection: 'asc' | 'desc' = 'asc';

  @ViewChild(MatSort) sort!: MatSort;

  constructor(private http: HttpClient, public router: Router) {}

  ngOnInit() {
    this.fetchCars();
  }

  async fetchCars() {
    try {
      const data = await firstValueFrom(this.http.get<any[]>('http://localhost:5073/api/cars'));
      this.dataSource.data = data || [];
      this.dataSource.sort = this.sort;
    } catch (error) {
      console.error('Error fetching cars:', error);
    }
  }

  onExport() {
    const worksheet = XLSX.utils.json_to_sheet(this.dataSource.data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Cars');
    XLSX.writeFile(workbook, 'Cars.xlsx');
  }

  sortData(column: string) {
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    this.dataSource.data = [...this.dataSource.data].sort((a, b) => {
      const aValue = a[column];
      const bValue = b[column];
      const multiplier = this.sortDirection === 'asc' ? 1 : -1;
      if (typeof aValue === 'string') return multiplier * aValue.localeCompare(bValue);
      if (typeof aValue === 'number') return multiplier * (aValue - bValue);
      if (aValue instanceof Date) return multiplier * (aValue.getTime() - bValue.getTime());
      return 0;
    });
    this.dataSource._updateChangeSubscription();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value?.trim().toLowerCase() || '';
    this.dataSource.filter = filterValue;
  }

  groupData(column: string) {
    if (!column) {
      this.fetchCars();
      return;
    }
    const grouped = this.groupBy(this.dataSource.data, column);
    this.dataSource.data = this.flattenGroupedData(grouped, column);
  }

  groupBy(data: any[], key: string): { [key: string]: any[] } {
    return data.reduce((acc, curr) => {
      (acc[curr[key]] = acc[curr[key]] || []).push(curr);
      return acc;
    }, {} as { [key: string]: any[] });
  }

  flattenGroupedData(grouped: { [key: string]: any[] }, key: string): any[] {
    const result: any[] = [];
    for (const [groupKey, groupItems] of Object.entries(grouped)) {
      result.push({ groupHeader: `${this.keyToLabel(key)}: ${groupKey}`, isGroup: true });
      if (Array.isArray(groupItems)) {
        result.push(...groupItems.map((item: any) => ({ ...item, isGroup: false })));
      }
    }
    return result;
  }

  isGroup = (_: number, row: any): boolean => row.isGroup === true;
  isData = (_: number, row: any): boolean => !row.isGroup;

  keyToLabel(key: string): string {
    const labels: { [key: string]: string } = {
      id: 'ID',
      carBrand: 'Brand',
      carColour: 'Colour',
      carPrice: 'Price',
      modelDate: 'Model Date',
      inStock: 'In Stock'
    };
    return labels[key] || key;
  }
}
