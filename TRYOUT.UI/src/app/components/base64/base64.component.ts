import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-base64',
  templateUrl: './base64.component.html',
  styleUrls: ['./base64.component.css']
})
export class Base64Component implements OnInit {

  encodedText: string = '';

  ngOnInit(): void {
    
  }

  onConvert(inputText: string) {
    if (inputText.trim().length == 0) return;

    console.log(inputText)
  }
}
