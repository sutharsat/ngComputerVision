export class OcrResult {
  language: string;
  detectedText: string;
  generatedId: string

  constructor() {
    this.language = '';
    this.detectedText = '';
    this.generatedId = '';
  }
}
