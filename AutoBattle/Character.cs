using System;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character : IComparable<Character>
    {
        private const float baseHealth = 100;
        private const float baseDamage = 20;
        private const int chanceToUseSkill = 30;

        private static Random random = new Random();

        public readonly string Name;
        public float Health { get; private set; }
        public readonly float OriginalHealth;
        public readonly float Damage;
        public readonly float DamageMultiplier;
        public Cell CurrentCell;
        public readonly int Index;
        public Character Target { get; set; }
        public readonly CharacterClass CharacterClass;
        public readonly CharacterClassParameters Parameters;
        public readonly CharacterSkills[] skills;
        public StatusEffect Status = StatusEffect.Normal;

        public Character(string name, int index, CharacterClass characterClass)
        {
            Name = name;
            Index = index;
            CharacterClass = characterClass;
            Parameters = GetCharacterClassParameters(CharacterClass);
            Health = Parameters.HealthModifier * baseHealth;
            Damage = Parameters.DamageModifier * baseDamage;
            DamageMultiplier = Parameters.DamageMultiplier;
            skills = Parameters.Skills;
            OriginalHealth = Health;
        }

        public void TakeDamage(float amount)
        {
            // Simplified
            Health -= amount;
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Status = StatusEffect.Dead;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Name} has been killed!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void WalkTo(Cell targetCell, char direction)
        {
            CurrentCell.Occupant = null;
            CurrentCell = targetCell;
            CurrentCell.Occupant = this;
            Console.WriteLine($"{Name} walked {direction}");
        }

        public void StartTurn(Grid grid)
        {
            // Can perform actions?
            if (Status == StatusEffect.Dead)
                return;
            if (Status == StatusEffect.Paralysed) {
                // Chance to leave paralysis is based on current health
                // 80% chance at 100% health => 30% at 0% health
                if (random.Next(100) < (int)(GetHealthPercentage() * 50) + 30) {
                    Status = StatusEffect.Normal;
                    Console.Write($"{Name} is no longer ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("paralyzed!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else {
                    Console.Write($"{Name} is ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("paralyzed");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" and cannot move!");
                    return;
                }
            }

            // First, try to attack anyone in adjacent cells
            if (IsCloseToTarget(grid)) {
                Attack();
                return;
            }
            // Otherwise, calculates in which direction this character should move to be closer to the target
            GetMovementDirection(grid, out char direction, out Cell targetCell);
            if (targetCell == null)
                Console.WriteLine($"{Name} couldn't find a direction to move to!\n");
            else
                WalkTo(targetCell, direction);
        }

        private void GetMovementDirection(Grid grid, out char direction, out Cell targetCell)
        {
            direction = ' ';
            targetCell = CurrentCell;
            // Move left
            if (CurrentCell.X > Target.CurrentCell.X) {
                targetCell = grid.Matrix[CurrentCell.Index - 1];
                direction = '\u2190';
            }
            // Move right
            else if (CurrentCell.X < Target.CurrentCell.X) {
                targetCell = grid.Matrix[CurrentCell.Index + 1];
                direction = '\u2192';
            }
            // Move up
            else if (CurrentCell.Y > Target.CurrentCell.Y) {
                targetCell = grid.Matrix[CurrentCell.Index - grid.Columns];
                direction = '\u2191';
            }
            // Move down
            else if (CurrentCell.Y < Target.CurrentCell.Y) {
                targetCell = grid.Matrix[CurrentCell.Index + grid.Columns];
                direction = '\u2193';
            }
        }

        // Check in x and y directions if target is close enough to be attacked
        private bool IsCloseToTarget(Grid grid)
        {
            if (Target.CurrentCell.Index == CurrentCell.Index + 1)
                return true;
            if (Target.CurrentCell.Index == CurrentCell.Index - 1)
                return true;
            if (Target.CurrentCell.Index == CurrentCell.Index + grid.Columns)
                return true;
            if (Target.CurrentCell.Index == CurrentCell.Index - grid.Columns)
                return true;
            return false;
        }

        private void Attack()
        {
            int damage;
            // See if skill will be used
            if (chanceToUseSkill < random.Next(100)) {
                // Get random skill
                CharacterSkills skill = skills[random.Next(skills.Length)];
                damage = (int)(random.Next(0, (int)skill.Damage) * skill.DamageMultiplier);

                Console.Write($"{Name} has used skill ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($" {skill.Name}!");
                Console.ForegroundColor = ConsoleColor.White;

                // Try to paralyze
                if (skill.ChanceToParalyze * 100 < random.Next(100)) {
                    Target.Status = StatusEffect.Paralysed;
                    Console.Write($"{Name} has ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("paralyzed");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" {Target.Name}!");
                }
            }
            // Otherwise, normal damage
            else {
                damage = (int)(random.Next(0, (int)Damage) * DamageMultiplier);
            }
            Console.WriteLine($"{Name} did {damage} damage to {Target.Name}");
            Target.TakeDamage(damage);
        }

        public float GetHealthPercentage() => Health / OriginalHealth;

        public int CompareTo(Character other)
        {
            return Index.CompareTo(other.Index);
        }
    }
}