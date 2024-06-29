import { Routes } from '@angular/router';
import { WordFormComponent } from './word-form/word-form.component';
import { WordListComponent } from './word-list/word-list.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './auth.guard';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'add', component: WordFormComponent, canActivate: [AuthGuard] }, // guard
    { path: 'list', component: WordListComponent, canActivate: [AuthGuard] }, // guard
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: '**', redirectTo: '/home' } // Handle 404 or fallback
];
