import { LoginnedTokens } from './loginned-tokens';

export class Loginned {
    tokens: LoginnedTokens;
    userId: string;
    role: string;
    permissions: string[];
    countUnreadNotifications: number;
}