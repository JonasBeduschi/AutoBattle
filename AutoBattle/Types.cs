namespace AutoBattle
{
    public class Types
    {
        /// <summary>Used to define most parameters of created characters, based on class</summary>
        public struct CharacterClassParameters
        {
            // Making everything public
            public CharacterClass CharacterClass;

            public float HealthModifier;
            public float DamageModifier;
            public float DamageMultiplier;
            public CharacterSkills[] Skills;

            // Adding constructor
            public CharacterClassParameters(CharacterClass characterClass, float healthModifier, float damageModifier, float damageMultiplier, CharacterSkills[] skills)
            {
                CharacterClass = characterClass;
                HealthModifier = healthModifier;
                DamageModifier = damageModifier;
                DamageMultiplier = damageMultiplier;
                Skills = skills;
            }

            public static CharacterClassParameters PaladinParameters
            {
                get => new CharacterClassParameters(CharacterClass.Paladin, 2.0f, 0.8f, 1.1f, new CharacterSkills[] { CharacterSkills.HeavyStrike });
            }

            public static CharacterClassParameters WarriorParameters
            {
                get => new CharacterClassParameters(CharacterClass.Warrior, 1.5f, 1.0f, 1.0f, new CharacterSkills[] { CharacterSkills.DoubleStrike });
            }

            public static CharacterClassParameters ClericParameters
            {
                get => new CharacterClassParameters(CharacterClass.Cleric, 1.2f, 1.1f, 1.1f, new CharacterSkills[] { CharacterSkills.HolyAttack });
            }

            public static CharacterClassParameters ArcherParameters
            {
                get => new CharacterClassParameters(CharacterClass.Archer, 0.8f, 2.0f, 1.2f, new CharacterSkills[] { CharacterSkills.AimedShot });
            }
        }

        public static CharacterClassParameters GetCharacterClassParameters(CharacterClass characterClass)
        {
            switch (characterClass) {
                case CharacterClass.Paladin:
                    return CharacterClassParameters.PaladinParameters;

                case CharacterClass.Warrior:
                    return CharacterClassParameters.WarriorParameters;

                case CharacterClass.Cleric:
                    return CharacterClassParameters.ClericParameters;

                case CharacterClass.Archer:
                    return CharacterClassParameters.ArcherParameters;

                default:
                    return CharacterClassParameters.PaladinParameters;
            }
        }

        // Changed GridBox to Cell. It's simpler and much more common when talking about matrices and grids
        // Make it a class. We need to access it as reference in the Matrix
        public class Cell
        {
            // It's a Cell, so it's quite clear what X and Y stand for
            public int X;

            public int Y;
            public int Index;
            public Character Occupant; // Able to tell who is in the cell, not only if
            public bool Occupied => Occupant != null;

            public Cell(int x, int y, int index)
            {
                X = x;
                Y = y;
                Index = index;
                Occupant = null;
            }

            public static Cell Zero => new Cell(0, 0, 0);
        }

        public struct CharacterSkills
        {
            // Making everything public
            public string Name;

            public float Damage;
            public float DamageMultiplier;
            public float ChanceToParalyze;

            // Adding constructor
            public CharacterSkills(string name, float damage, float damageMultiplier, float chanceToParalyze)
            {
                Name = name;
                Damage = damage;
                DamageMultiplier = damageMultiplier;
                ChanceToParalyze = chanceToParalyze;
            }

            // Adding sample skills
            public static CharacterSkills HolyAttack { get => new CharacterSkills("Holy Attack", 10, 1f, .8f); }
            public static CharacterSkills HeavyStrike { get => new CharacterSkills("Heavy Strike", 15, 1.8f, .4f); }
            public static CharacterSkills DoubleStrike { get => new CharacterSkills("Double Strike", 20, 2f, .2f); }
            public static CharacterSkills AimedShot { get => new CharacterSkills("Aimed Shot", 20, 1.5f, .5f); }
        }

        public enum CharacterClass : byte
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

        public enum StatusEffect : byte
        {
            Normal = 0,
            Dead = 1,
            Paralysed = 2,
        }
    }
}