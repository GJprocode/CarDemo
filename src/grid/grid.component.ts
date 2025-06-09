import { Component } from '@angular/core';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { DxDataGridModule } from 'devextreme-angular';

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [DxDataGridModule],
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent {
  dataSource: any;
  auto: string | number | (() => string | number) | undefined;

  constructor() {
    this.dataSource = AspNetData.createStore({
      key: 'id',
      loadUrl: 'http://localhost:5073/api/cars/grid',
      insertUrl: 'http://localhost:5073/api/cars',
      updateUrl: 'http://localhost:5073/api/cars',
      deleteUrl: 'http://localhost:5073/api/cars',
      onBeforeSend(method, ajaxOptions) {
        ajaxOptions.contentType = 'application/json';
        ajaxOptions.data = JSON.stringify(ajaxOptions.data); // Sends PascalCase correctly
      }
    });
  }
}
