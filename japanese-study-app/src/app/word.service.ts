import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Word {
  id: number;
  jpWord: string;
  pronunciation: string;
  enMeaning: string;
  thMeaning: string;
}

@Injectable({
  providedIn: 'root'
})
export class WordService {
  private apiUrl = 'http://localhost:5051/api/words';  // Adjust the URL if necessary

  constructor(private http: HttpClient) { }

  getWords(): Observable<Word[]> {
    return this.http.get<Word[]>(this.apiUrl);
  }

  getWord(id: number): Observable<Word> {
    return this.http.get<Word>(`${this.apiUrl}/${id}`);
  }

  addWord(word: Word): Observable<Word> {
    return this.http.post<Word>(this.apiUrl, word);
  }

  updateWord(word: Word): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${word.id}`, word);
  }

  deleteWord(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
