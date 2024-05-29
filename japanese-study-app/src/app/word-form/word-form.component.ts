import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WordService, Word } from '../word.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-word-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule],
  templateUrl: './word-form.component.html',
  styleUrls: ['./word-form.component.css']
})
export class WordFormComponent implements OnInit, OnDestroy {
  wordForm: FormGroup = new FormGroup({
    jpWord: new FormControl(null, [Validators.required]),
    pronunciation: new FormControl(null, [Validators.required]),
    enMeaning: new FormControl(null, [Validators.required]),
    thMeaning: new FormControl(null, [Validators.required]),
  });

  constructor(private wordService: WordService, private router: Router) { }

  ngOnInit(): void { }

  submit() {
    if (this.wordForm.valid) {
      const newWord: Word = this.wordForm.value;
      this.wordService.addWord(newWord).subscribe(() => {
        this.router.navigate(['/list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/list']);
  }

  ngOnDestroy(): void { }
}
