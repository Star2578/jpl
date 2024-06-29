import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service'; // Import your AuthService

export interface Word {
  id: number;
  jpWord: string;
  pronunciation: string;
  enMeaning: string;
  thMeaning: string;
  userId: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class WordService {
  private apiUrl = 'http://localhost:5051/api/words';  // Adjust the URL if necessary

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    });
  }

  getWords(): Observable<Word[]> {
    return this.http.get<Word[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  getWord(id: number): Observable<Word> {
    return this.http.get<Word>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  addWord(word: Word): Observable<Word> {
    return this.http.post<Word>(this.apiUrl, word, { headers: this.getHeaders() });
  }

  updateWord(word: Word): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${word.id}`, word, { headers: this.getHeaders() });
  }

  deleteWord(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}
