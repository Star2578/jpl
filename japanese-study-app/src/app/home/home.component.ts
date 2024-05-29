import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, HttpClientModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  id: string = 'L2i0i9gWE00';
  videoPool: string[] = [
    'L2i0i9gWE00', 'MSnLgdQFk1c',
    '7FDRQifEMUQ', '5RSHNMkKGLs',
    'SDk1RA4g8CA', 'IwHwv-lcxi4',
    'yiHexptwQ2Q', 'NF8c5pXx-Xc',
    'rXRvs_FrwEk', 'omckrS77vDo',
  ]

  constructor(public sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.selectRandomVideo();
  }

  selectRandomVideo(): void {
    const randomIndex = Math.floor(Math.random() * this.videoPool.length);
    this.id = this.videoPool[randomIndex];
  }
}
