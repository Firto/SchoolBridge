import { LoginnedTokens } from './loginned-tokens';

export class Loginned {
    tokens: LoginnedTokens
    role: string;
    permissions: string[];
    countUnreadNotifications: number;
}