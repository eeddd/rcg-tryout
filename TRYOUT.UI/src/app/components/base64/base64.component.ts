import { Component, OnInit } from '@angular/core';
import { Base64Service } from 'src/app/services/base64.service';

@Component({
  selector: 'app-base64',
  templateUrl: './base64.component.html',
  styleUrls: ['./base64.component.css']
})
export class Base64Component implements OnInit {

  encodedText: string = '';

  isBusy: boolean = false;

  constructor(private service: Base64Service) { }

  ngOnInit(): void {
    this.service.startConnection();
    this.service.addListeners();
    this.service.getPartialData().subscribe(data => this.encodedText += data);
    this.service.getEncodedData().subscribe(data => this.encodedText = data);
    this.service.getBusyData().subscribe(data => this.isBusy = data);
  }

  onConvert(inputText: string) {
    if (inputText.trim().length == 0) return;

    this.encodedText = "";
    this.service.convertText(inputText)
  }

  onCancel() {
    this.service.cancelConversion();
  }
}
