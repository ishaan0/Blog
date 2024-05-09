import { Component } from '@angular/core';

import { SharedModule } from './shared/modules/shared.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Blog.Client';
}
