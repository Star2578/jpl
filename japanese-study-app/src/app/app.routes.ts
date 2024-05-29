import { Routes } from '@angular/router';
import { WordFormComponent } from './word-form/word-form.component';
import { WordListComponent } from './word-list/word-list.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'add', component: WordFormComponent },
    { path: 'list', component: WordListComponent },
];
