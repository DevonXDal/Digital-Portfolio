export class ContactMessage {
    constructor(
        public email: string,
        public name: string,
        public message: string,
        public phone?: string,
    ) {}
}