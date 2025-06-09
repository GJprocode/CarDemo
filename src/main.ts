import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app/app.routes';
import { licenseKey } from '../src/devextreme-license'; // Your local, gitignored file
import config from 'devextreme/core/config';

// ✅ Register DevExtreme license (from 30-day trial or full license)
config({ licenseKey });

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi())
  ]
}).catch(err => console.error(err));