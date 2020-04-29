import { LoginnedTokens } from './loginned-tokens';
import { DataBaseSource } from 'src/app/Modules/db-notification/Sources/database-source';

export class Loginned {
    tokens: LoginnedTokens
    role: string;
    panels: string[];
    permissions: string[];
    notifications: DataBaseSource[];
}