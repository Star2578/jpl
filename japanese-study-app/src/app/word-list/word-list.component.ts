import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Word, WordService } from '../word.service';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-word-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule, ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './word-list.component.html',
  styleUrl: './word-list.component.css'
})
export class WordListComponent {
  searchControl: FormControl = new FormControl('');
  words: Word[] = [];
  filteredWords: Word[] = [];
  private subscriptions: Subscription[] = [];

  faTrash = faTrash;

  constructor(private wordService: WordService) { }

  ngOnInit(): void {
    // Load words initially
    this.loadWords();

    // Subscribe to changes in the search input
    this.subscriptions.push(
      this.searchControl.valueChanges.subscribe(() => {
        this.filterWords();
      })
    );
  }

  loadWords() {
    this.wordService.getWords().subscribe((words) => {
      this.words = words;
      this.filterWords(); // Apply search filtering
    });
  }

  confirmDelete(wordId: number): void {
    const isConfirmed = window.confirm('Are you sure you want to delete this word?');
    if (isConfirmed) {
      this.deleteWord(wordId);
    }
  }

  deleteWord(id: number): void {
    this.wordService.deleteWord(id).subscribe(() => {
      window.location.reload();
    });
  }

  filterWords() {
    const searchTerm = this.searchControl.value.toLowerCase();
    this.filteredWords = this.words.filter(word =>
      word.jpWord.toLowerCase().includes(searchTerm) ||
      word.enMeaning.toLowerCase().includes(searchTerm) ||
      word.thMeaning.toLowerCase().includes(searchTerm) ||
      word.pronunciation.toLowerCase().includes(searchTerm)
    );
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions to prevent memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
